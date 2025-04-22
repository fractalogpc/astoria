using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class LightningMeshGeneration : MonoBehaviour {
    public Vector3 goalPosition;
    public Vector3 spawnPosition;

    // public GameObject light;

    [SerializeField] public int numSegments = 30;
    public float hitTimeMax;
    [SerializeField] private float spawnHeight = 10f;
    [SerializeField] private float lightningSize = .1f;
    [SerializeField] private Material lightningMaterial;
    [SerializeField] private AnimationCurve animCurve;
    public float downStrength;
    public float brightness = 0;
    public float brightnessMultiplier = 1;

    public Vector3[] Vertex = new Vector3[30];

    public List<Vector3> SubVertexDirection;
    public List<Vector3> SubVertexStart;
    public List<Vector3> SubVertexGoal;
    public float SubVertexMaxSize;
    public float SubVertexRandomizationStrength;
    public List<Vector3[]> SubVertex = new List<Vector3[]>();

    [Range(0, 1)] public float lerp = .75f;

    public Mesh mesh;

    private bool Spawned = false;

    void Awake() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private float timer = 0;
    void Update() {
        if (Spawned)
        {
            timer += Time.deltaTime;
            {
                brightness = animCurve.Evaluate(timer / hitTimeMax) * brightnessMultiplier;

                if (timer > hitTimeMax)
                {
                    ClearLightning();
                    
                    Destroy(this.gameObject);
                }
                lightningMaterial.SetFloat("_Brightness", brightness);
            }
        }

    }

    public void Initialize(Vector3 start, Vector3 goal) {
        spawnPosition = start;
        goalPosition = goal;
        SpawnLightning();

        Spawned = true;
    }

    private void SpawnLightning() {
        // Debug.Log("Spawning lightning");

        SubVertexDirection.Clear();
        SubVertexStart.Clear();
        SubVertexGoal.Clear();
        SubVertex.Clear();
        mesh.Clear();
        FormLightning();
        FormMesh(Vertex);
        for (int i = 0; i < SubVertex.Count; i++)
        {
            FormMesh(SubVertex[i]);
        }

        // light.SetActive(true);
        lightningMaterial.SetFloat("_Brightness", brightness);

        // Random duration
        //float duration = Random.value + 1f;
        //Invoke("ClearLightning", duration);
    }

    private void ClearLightning() {
        // light.SetActive(false);
        mesh.Clear();
    }

    void FormLightning() {
        //main path
        for (int i = 0; i < numSegments; i++)
        {
            Vector3 line = Random.onUnitSphere * Mathf.Clamp(Vector3.Distance(Vector3.Lerp(spawnPosition, goalPosition, (float)i / numSegments), goalPosition), .1f, 50);
            Vertex[i] = Vector3.Lerp(spawnPosition, goalPosition, (float)i / numSegments) + new Vector3(line.x, Mathf.Abs(line.y), line.z);
            //create sub-branch
            if (i > 1)
            {
                Vector3 tangent = goalPosition - Vertex[i];
                Vector3 biNormal = Vector3.Normalize(Vector3.Cross(Vertex[i] - Vertex[i - 1], tangent + new Vector3(100, -12, 402)));
                if (Random.Range(0f, 1f) > .5f)
                {
                    SubVertexDirection.Add(biNormal * 2f);
                    SubVertexStart.Add(Vertex[i]);
                    SubVertex.Add(new Vector3[8]);
                }
            }


        }

        //sub-branch
        for (int i = 0; i < SubVertexDirection.Count; i++)
        {
            Vector3 randomPos = Random.insideUnitSphere;
            randomPos.y = -Mathf.Abs(randomPos.y);
            randomPos = Vector3.Lerp(randomPos, Vector3.Normalize(goalPosition - spawnPosition), downStrength);
            SubVertexGoal.Add(SubVertexStart[i] + randomPos * Vector3.Distance(goalPosition, SubVertexStart[i]));
            for (int j = 0; j < 8; j++)
            {
                if (j == 0)
                {
                    SubVertex[i][j] = SubVertexStart[i];
                }
                else
                {
                    Vector3 randomDirection = Random.onUnitSphere * Mathf.Clamp(Vector3.Distance(goalPosition, SubVertexStart[i]) / 5f, 1f, SubVertexMaxSize) * SubVertexRandomizationStrength;
                    randomDirection.y = -Mathf.Abs(randomDirection.y);
                    Vector3 point = Vector3.Lerp(SubVertexStart[i], SubVertexGoal[i], (float)j / 8) + Vector3.Lerp(randomDirection, Vector3.Normalize(SubVertexGoal[i] - SubVertexStart[i]), lerp) * Mathf.Clamp(Vector3.Distance(goalPosition, SubVertexStart[i]) / 5f, 1f, SubVertexMaxSize);
                    SubVertex[i][j] = point;
                }

            }
        }
    }

    void FormMesh(Vector3[] Vectors) {
        if (mesh.vertices.Length < 1)
        {
            mesh = new Mesh {
                name = "Procedural Mesh"
            };
        }

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        int offset = mesh.vertices.Length;

        for (int i = 0; i < Vectors.Length; i++)
        {
            Vector3 nextVertex = i < Vectors.Length - 1 ? Vectors[i + 1] : goalPosition;
            Vector3 Tangent = nextVertex - Vectors[i];
            Vector3 BiNormal = Vector3.Normalize(Vector3.Cross(Tangent, Tangent + new Vector3(1231, 12f, -1203f)));
            Vector3 RotNormal = Quaternion.AngleAxis(90, Tangent) * BiNormal;

            vertices.Add((Vectors[i] - spawnPosition) + BiNormal * ((Vectors.Length - i) * lightningSize));
            vertices.Add((Vectors[i] - spawnPosition) + RotNormal * ((Vectors.Length - i) * lightningSize));
            vertices.Add((Vectors[i] - spawnPosition) - BiNormal * ((Vectors.Length - i) * lightningSize));
            vertices.Add((Vectors[i] - spawnPosition) - RotNormal * ((Vectors.Length - i) * lightningSize));
        }

        if (offset < 1)
        {
            vertices.Add((goalPosition - spawnPosition) + new Vector3(1f, 0, 0));
            vertices.Add((goalPosition - spawnPosition) + new Vector3(0, 0, 1f));
            vertices.Add((goalPosition - spawnPosition) + new Vector3(-1f, 0, 0));
            vertices.Add((goalPosition - spawnPosition) + new Vector3(0, 0, -1f));
        }


        for (int i = 0; i < vertices.Count - 5; i++)
        {
            if (i % 4 == 3)
            {
                triangles.Add(offset + i);
                triangles.Add(offset + i + 4);
                triangles.Add(offset + i + 1);
                triangles.Add(offset + i);
                triangles.Add(offset + i + 1);
                triangles.Add(offset + i - 3);
            }
            else
            {
                triangles.Add(offset + i);
                triangles.Add(offset + i + 4);
                triangles.Add(offset + i + 5);
                triangles.Add(offset + i);
                triangles.Add(offset + i + 5);
                triangles.Add(offset + i + 1);
            }

        }

        if (mesh.vertices.Length < 1)
        {
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
        }
        else
        {
            mesh.vertices = mesh.vertices.Concat(vertices).ToArray();
            mesh.triangles = mesh.triangles.Concat(triangles).ToArray();
        }

        GetComponent<MeshFilter>().mesh = mesh;
    }
}