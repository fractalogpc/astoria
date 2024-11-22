using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
public class RiverFromTexture : EditorWindow
{

    private Texture2D inputTexture;
    private Texture2D heightmapTexture;
    private Texture2D depthTexture;
    private float heightmapScale = 1.0f;
    private float xSize = 10.0f;
    private float ySize = 10.0f;
    private float riverDownturn = 0.5f;
    private string savePath = "Assets/GeneratedRiver.asset";

    [MenuItem("Tools/River From Texture")]
    public static void ShowWindow() {
        GetWindow<RiverFromTexture>("River From Texture");
    }

    private void OnGUI() {
        GUILayout.Label("River From Texture", EditorStyles.boldLabel);

        inputTexture = (Texture2D)EditorGUILayout.ObjectField("Input Texture", inputTexture, typeof(Texture2D), false);
        heightmapTexture = (Texture2D)EditorGUILayout.ObjectField("Heightmap Texture", heightmapTexture, typeof(Texture2D), false);
        depthTexture = (Texture2D)EditorGUILayout.ObjectField("Depth Texture", depthTexture, typeof(Texture2D), false);
        heightmapScale = EditorGUILayout.FloatField("Heightmap Scale", heightmapScale);
        xSize = EditorGUILayout.FloatField("X Size", xSize);
        ySize = EditorGUILayout.FloatField("Y Size", ySize);
        riverDownturn = EditorGUILayout.FloatField("River Downturn", riverDownturn);
        savePath = EditorGUILayout.TextField("Save Path", savePath);
        if (GUILayout.Button("Generate and Save Mesh")) {
            if (inputTexture == null || heightmapTexture == null) {
                EditorUtility.DisplayDialog("Error", "Please assign a texture before generating a mesh.", "OK");
                return;
            }

            GenerateAndSaveMesh();
        }
    }

    private void GenerateAndSaveMesh() {
        Mesh mesh = GenerateMeshFromTexture(inputTexture, heightmapTexture, heightmapScale, xSize, ySize, riverDownturn);

        if (mesh != null) {
            SaveMeshAsset(mesh);
            EditorUtility.DisplayDialog("Success", "Mesh saved successfully to " + savePath, "OK");
        }
        else {
            EditorUtility.DisplayDialog("Error", "Mesh generation failed.", "OK");
        }
    }

    private Mesh GenerateMeshFromTexture(Texture2D texture, Texture2D heightmap, float heightmapScale, float xSize, float ySize, float riverDownturn) {
        // Modify river mask texture to a distance field (distance to nearest water pixel)
        Color[] pixels = texture.GetPixels();
        int width = texture.width;
        int height = texture.height;
        float[] distances = new float[width * height];

        int searchDistance = 2;

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                int i = y * width + x;
                if (pixels[i].r > 0) {
                    distances[i] = 0;
                } else {
                    distances[i] = float.MaxValue;
                }
            }
        }

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                int i = y * width + x;
                if (distances[i] == 0) {
                    continue;
                }

                for (int dy = -searchDistance; dy <= searchDistance; dy++) {
                    for (int dx = -searchDistance; dx <= searchDistance; dx++) {
                        if (dx == 0 && dy == 0) {
                            continue;
                        }

                        int nx = x + dx;
                        int ny = y + dy;
                        if (nx < 0 || nx >= width || ny < 0 || ny >= height) {
                            continue;
                        }

                        int ni = ny * width + nx;
                        float d = distances[ni] + Mathf.Sqrt(dx * dx + dy * dy);
                        if (d < distances[i]) {
                            distances[i] = d;
                        }
                    }
                }
            }
        }

        float maxDistance = 4;
        
        Texture2D distanceFieldTexture = new Texture2D(width, height);

        for (int i = 0; i < distances.Length; i++) {
            float d = Mathf.Min(distances[i], maxDistance);
            distanceFieldTexture.SetPixel(i % width, i / width, new Color(distances[i] / (maxDistance + 1), 0, 0));
        }

        Mesh mesh = new Mesh();

        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        float xStep = xSize / width;
        float yStep = ySize / height;

        Vector3[] vertices = new Vector3[(width + 1) * (height + 1)];
        bool[] water = new bool[(width + 1) * (height + 1)];
        int[] triangles = new int[width * height * 6];

        for (int y = 0; y <= height; y++) {
            for (int x = 0; x <= width; x++) {
                float u = (float)x / width;
                float v = (float)y / height;

                float heightValue = heightmap.GetPixelBilinear(u, v).r;
                Color pixel = distanceFieldTexture.GetPixelBilinear(u, v);
                water[y * (width + 1) + x] = pixel.r < 1;
                float heightOffset = pixel.r * riverDownturn;

                float depthValue = depthTexture.GetPixelBilinear(u, v).r;
                if (depthValue <= 0) {
                    // Find nearest pixel with depth value and use that
                    int nearestX = x;
                    int nearestY = y;
                    float nearestDistance = float.MaxValue;
                    for (int dy = -searchDistance; dy <= searchDistance; dy++) {
                        for (int dx = -searchDistance; dx <= searchDistance; dx++) {
                            int nx = x + dx;
                            int ny = y + dy;
                            if (nx < 0 || nx >= width || ny < 0 || ny >= height) {
                                continue;
                            }

                            float d = depthTexture.GetPixelBilinear((float)nx / width, (float)ny / height).r;
                            if (d > 0) {
                                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                                if (distance < nearestDistance) {
                                    nearestX = nx;
                                    nearestY = ny;
                                    nearestDistance = distance;
                                }
                            }
                        }
                    }

                    depthValue = depthTexture.GetPixelBilinear((float)nearestX / width, (float)nearestY / height).r;
                }
                heightValue += depthValue;

                vertices[y * (width + 1) + x] = new Vector3(x * xStep, heightValue * heightmapScale - heightOffset, y * yStep);
            }
        }

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                int i = y * width + x;
                int vi = y * (width + 1) + x;

                triangles[i * 6 + 0] = vi;
                triangles[i * 6 + 1] = vi + width + 1;
                triangles[i * 6 + 2] = vi + width + 2;

                triangles[i * 6 + 3] = vi;
                triangles[i * 6 + 4] = vi + width + 2;
                triangles[i * 6 + 5] = vi + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        int waterCount = 0;
        for (int i = 0; i < water.Length; i++) {
            if (water[i]) {
                waterCount++;
            }
        }

        Vector3[] waterVertices = new Vector3[waterCount];
        int[] waterVerticesMap = new int[vertices.Length];

        int waterIndex = 0;
        for (int i = 0; i < water.Length; i++) {
            if (water[i]) {
                waterVertices[waterIndex] = vertices[i];
                waterVerticesMap[i] = waterIndex;
                waterIndex++;
            } else {
                waterVerticesMap[i] = -1;
            }
        }

        List<int> waterTriangles = new List<int>();
        for (int i = 0; i < triangles.Length; i += 3) {
            int a = triangles[i];
            int b = triangles[i + 1];
            int c = triangles[i + 2];

            if (water[a] && water[b] && water[c]) {
                waterTriangles.Add(waterVerticesMap[a]);
                waterTriangles.Add(waterVerticesMap[b]);
                waterTriangles.Add(waterVerticesMap[c]);
            }
        }

        mesh.triangles = new int[0];
        mesh.vertices = waterVertices;
        mesh.triangles = waterTriangles.ToArray();

        mesh.RecalculateNormals();

        return mesh;
    }

    private void SaveMeshAsset(Mesh mesh) {
        string directory = Path.GetDirectoryName(savePath);
        if (!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }

        AssetDatabase.CreateAsset(mesh, savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
#endif
