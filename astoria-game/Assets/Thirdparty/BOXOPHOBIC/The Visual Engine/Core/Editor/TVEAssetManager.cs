﻿// Cristian Pop - https://boxophobic.com/

using UnityEngine;
using UnityEditor;
using Boxophobic.StyledGUI;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace TheVisualEngine
{
    public class TVEAssetManager : EditorWindow
    {
        float GUI_HALF_EDITOR_WIDTH = 200;
        float GUI_FULL_EDITOR_WIDTH = 200;

        string[] SeletionEnum = new string[]
        {
        "Selected Folder", "Selected Assets",
        };

        int selectionIndex = 1;

        List<string> allAssetsPaths;

        GUIStyle stylePopup;

        Color bannerColor;
        string bannerText;
        static TVEAssetManager window;
        Vector2 scrollPosition = Vector2.zero;

        [MenuItem("Window/BOXOPHOBIC/The Visual Engine/Asset Manager", false, 2005)]
        public static void ShowWindow()
        {
            window = GetWindow<TVEAssetManager>(false, "Asset Manager", true);
            window.minSize = new Vector2(400, 300);
        }

        void OnEnable()
        {
            bannerColor = new Color(0.890f, 0.745f, 0.309f);
            bannerText = "Assets Manager";
        }

        void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false);

            SetGUIStyles();

            GUI_HALF_EDITOR_WIDTH = (this.position.width / 2.0f - 24) - 5;
            GUI_FULL_EDITOR_WIDTH = this.position.width - 40;

            GUILayout.Space(10);
            TVEUtils.DrawToolbar(0, -1);
            StyledGUI.DrawWindowBanner(bannerColor, bannerText);

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Selection Mode", GUILayout.Width(GUI_HALF_EDITOR_WIDTH));
            selectionIndex = EditorGUILayout.Popup(selectionIndex, SeletionEnum, stylePopup);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            StyledGUI.DrawWindowCategory("Processing Settings");
            GUILayout.Space(10);

            if (GUILayout.Button("Validate TVE Materials", GUILayout.Height(24)))
            {
                allAssetsPaths = new List<string>();

                GetAssetsPaths("", "*.mat");

                for (int i = 0; i < allAssetsPaths.Count; i++)
                {
                    var assetPath = allAssetsPaths[i];

                    TVEUtils.ValidateMaterial(assetPath, false);

                    var progress = (float)i * 1.0f / (float)allAssetsPaths.Count;
                    EditorUtility.DisplayProgressBar("The Visual Engine", "Processing " + Path.GetFileName(assetPath), progress);
                }

                EditorUtility.ClearProgressBar();

                Debug.Log("<b>[The Visual Engine]</b> " + allAssetsPaths.Count.ToString() + " materials have been processed.");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Validate TVE Elements", GUILayout.Height(24)))
            {
                allAssetsPaths = new List<string>();

                GetAssetsPaths("", "*.mat");

                for (int i = 0; i < allAssetsPaths.Count; i++)
                {
                    var assetPath = allAssetsPaths[i];

                    TVEUtils.ValidateMaterial(assetPath, false);

                    var progress = (float)i * 1.0f / (float)allAssetsPaths.Count;
                    EditorUtility.DisplayProgressBar("The Visual Engine", "Processing " + Path.GetFileName(assetPath), progress);
                }

                EditorUtility.ClearProgressBar();

                Debug.Log("<b>[The Visual Engine]</b> " + allAssetsPaths.Count.ToString() + " elements have been processed.");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            //if (GUILayout.Button("Validate TVE Models", GUILayout.Height(24)))
            //{
            //    allAssetsPaths = new List<string>();

            //    GetAssetsPaths("TVE Model", "*.asset");

            //    for (int i = 0; i < allAssetsPaths.Count; i++)
            //    {
            //        var assetPath = allAssetsPaths[i];

            //        TVEUtils.ValidateModel(assetPath, false);

            //        var progress = (float)i * 1.0f / (float)allAssetsPaths.Count;
            //        EditorUtility.DisplayProgressBar("The Visual Engine", "Processing " + Path.GetFileName(assetPath), progress);
            //    }

            //    EditorUtility.ClearProgressBar();

            //    Debug.Log("<b>[The Visual Engine]</b> " + allAssetsPaths.Count.ToString() + " models have been processed.");

            //    AssetDatabase.SaveAssets();
            //    AssetDatabase.Refresh();
            //}

            GUILayout.Space(10);

            if (GUILayout.Button("Mark Model As Readable", GUILayout.Height(24)))
            {
                allAssetsPaths = new List<string>();

                GetAssetsPaths("TVE Model", "*.asset");

                for (int i = 0; i < allAssetsPaths.Count; i++)
                {
                    var assetPath = allAssetsPaths[i];

                    string fileText = File.ReadAllText(assetPath);
                    fileText = fileText.Replace("m_IsReadable: 0", "m_IsReadable: 1");
                    File.WriteAllText(assetPath, fileText);

                    //Not working for some reasons
                    //var meshOriginal = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);

                    //var meshInstance = Instantiate(meshOriginal);
                    //meshInstance.name = meshOriginal.name;

                    //meshInstance.UploadMeshData(false);
                    //meshOriginal.Clear();

                    //EditorUtility.CopySerialized(meshInstance, meshOriginal);

                    var progress = (float)i * 1.0f / (float)allAssetsPaths.Count;
                    EditorUtility.DisplayProgressBar("The Visual Engine", "Processing " + Path.GetFileName(assetPath), progress);
                }

                EditorUtility.ClearProgressBar();

                Debug.Log("<b>[The Visual Engine]</b> " + allAssetsPaths.Count.ToString() + " meshes have been processed.");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Mark Model As Non Readable", GUILayout.Height(24)))
            {
                allAssetsPaths = new List<string>();

                GetAssetsPaths("TVE Model", "*.asset");

                for (int i = 0; i < allAssetsPaths.Count; i++)
                {
                    var assetPath = allAssetsPaths[i];

                    string fileText = File.ReadAllText(assetPath);
                    fileText = fileText.Replace("m_IsReadable: 1", "m_IsReadable: 0");
                    File.WriteAllText(assetPath, fileText);

                    var progress = (float)i * 1.0f / (float)allAssetsPaths.Count;
                    EditorUtility.DisplayProgressBar("The Visual Engine", "Processing " + Path.GetFileName(assetPath), progress);
                }

                EditorUtility.ClearProgressBar();

                Debug.Log("<b>[The Visual Engine]</b> " + allAssetsPaths.Count.ToString() + " meshes have been processed.");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Recalcuate Mesh Normals", GUILayout.Height(24)))
            {
                allAssetsPaths = new List<string>();

                GetAssetsPaths("TVE Model", "*.asset");

                for (int i = 0; i < allAssetsPaths.Count; i++)
                {
                    var assetPath = allAssetsPaths[i];

                    var meshOriginal = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);

                    var meshInstance = Instantiate(meshOriginal);
                    meshInstance.name = meshOriginal.name;

                    meshInstance.RecalculateNormals();
                    meshOriginal.Clear();

                    EditorUtility.CopySerialized(meshInstance, meshOriginal);

                    var progress = (float)i * 1.0f / (float)allAssetsPaths.Count;
                    EditorUtility.DisplayProgressBar("The Visual Engine", "Processing " + Path.GetFileName(assetPath), progress);
                }

                EditorUtility.ClearProgressBar();

                Debug.Log("<b>[The Visual Engine]</b> " + allAssetsPaths.Count.ToString() + " meshes have been processed.");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Recalcuate Mesh Tangents", GUILayout.Height(24)))
            {
                allAssetsPaths = new List<string>();

                GetAssetsPaths("TVE Model", "*.asset");

                for (int i = 0; i < allAssetsPaths.Count; i++)
                {
                    var assetPath = allAssetsPaths[i];

                    var meshOriginal = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);

                    var meshInstance = Instantiate(meshOriginal);
                    meshInstance.name = meshOriginal.name;

                    meshInstance.RecalculateTangents();
                    meshOriginal.Clear();

                    EditorUtility.CopySerialized(meshInstance, meshOriginal);

                    var progress = (float)i * 1.0f / (float)allAssetsPaths.Count;
                    EditorUtility.DisplayProgressBar("The Visual Engine", "Processing " + Path.GetFileName(assetPath), progress);
                }

                EditorUtility.ClearProgressBar();

                Debug.Log("<b>[The Visual Engine]</b> " + allAssetsPaths.Count.ToString() + " meshes have been processed.");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Fix Legacy Model Extra UV", GUILayout.Height(24)))
            {
                allAssetsPaths = new List<string>();

                GetAssetsPaths("TVE Model", "*.asset");

                for (int i = 0; i < allAssetsPaths.Count; i++)
                {
                    var assetPath = allAssetsPaths[i];

                    if (Path.GetFullPath(assetPath).Length > 256)
                    {
                        Debug.Log("<b>[The Visual Engine]</b> " + assetPath + " could not be upgraded because the file path is too long! To fix the issue, rename the folders and file names, then go to Hub > Show Advanced Settings > Validate All Project Meshes to re-process the meshes!");
                        return;
                    }

                    var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);

                    if (mesh == null)
                    {
                        Debug.Log("<b>[The Visual Engine]</b> " + assetPath + " could not be upgraded because the mesh is null!");
                        return;
                    }

                    var meshName = mesh.name;

                    var instanceMesh = UnityEngine.Object.Instantiate(mesh);
                    instanceMesh.name = meshName;

                    if (instanceMesh == null)
                    {
                        Debug.Log("<b>[The Visual Engine]</b> " + assetPath + " could not be upgraded because the mesh is null!");
                        continue;
                    }

                    var vertexCount = mesh.vertexCount;
                    var UV2 = new List<Vector4>(vertexCount);
                    var newUV3 = new List<Vector2>(vertexCount);

                    mesh.GetUVs(1, UV2);

                    if (UV2.Count != 0)
                    {
                        for (int v = 0; v < vertexCount; v++)
                        {
                            newUV3.Add(new Vector2(UV2[v].z, UV2[v].w));
                        }
                    }

                    instanceMesh.SetUVs(2, newUV3);

                    instanceMesh.SetUVs(2, newUV3);

                    mesh.Clear();

                    EditorUtility.CopySerialized(instanceMesh, mesh);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    TVEUtils.SetLabel(assetPath);

                    UV2.Clear();
                    newUV3.Clear();

                    var progress = (float)i * 1.0f / (float)allAssetsPaths.Count;
                    EditorUtility.DisplayProgressBar("The Visual Engine", "Processing " + Path.GetFileName(assetPath), progress);
                }

                EditorUtility.ClearProgressBar();

                Debug.Log("<b>[The Visual Engine]</b> " + allAssetsPaths.Count.ToString() + " models have been processed.");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Delete Backup Prefabs", GUILayout.Height(24)))
            {
                allAssetsPaths = new List<string>();

                GetAssetsPaths("TVE Backup", "*.prefab");

                for (int i = 0; i < allAssetsPaths.Count; i++)
                {
                    var assetPath = allAssetsPaths[i];

                    FileUtil.DeleteFileOrDirectory(assetPath);
                    FileUtil.DeleteFileOrDirectory(assetPath + ".meta");

                    var progress = (float)i * 1.0f / (float)allAssetsPaths.Count;
                    EditorUtility.DisplayProgressBar("The Visual Engine", "Processing " + Path.GetFileName(assetPath), progress);
                }

                EditorUtility.ClearProgressBar();

                Debug.Log("<b>[The Visual Engine]</b> " + allAssetsPaths.Count.ToString() + " backups have been deleted.");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Delete Prefab Component", GUILayout.Height(24)))
            {
                allAssetsPaths = new List<string>();

                GetAssetsPaths("", "*.prefab");

                for (int i = 0; i < allAssetsPaths.Count; i++)
                {
                    var assetPath = allAssetsPaths[i];

                    var prefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                    if (prefabObject.GetComponent<TVEPrefab>() != null)
                    {
                        var prefabInstance = Instantiate(prefabObject);

                        DestroyImmediate(prefabInstance.GetComponent<TVEPrefab>(), true);

                        PrefabUtility.SaveAsPrefabAssetAndConnect(prefabInstance, assetPath, InteractionMode.AutomatedAction);

                        var progress = (float)i * 1.0f / (float)allAssetsPaths.Count;
                        EditorUtility.DisplayProgressBar("The Visual Engine", "Processing " + Path.GetFileName(assetPath), progress);
                    }
                }

                EditorUtility.ClearProgressBar();

                Debug.Log("<b>[The Visual Engine]</b> " + allAssetsPaths.Count.ToString() + " prefabs have been processed.");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Delete Converted Assets", GUILayout.Height(24)))
            {
                allAssetsPaths = new List<string>();

                GetAssetsPaths("TVE Model", "*.asset");
                GetAssetsPaths("TVE Material", "*.mat");
                GetAssetsPaths("TVE Texture", "*.png");
                GetAssetsPaths("TVE Texture", "*.tga");

                for (int i = 0; i < allAssetsPaths.Count; i++)
                {
                    var assetPath = allAssetsPaths[i];

                    FileUtil.DeleteFileOrDirectory(assetPath);
                    FileUtil.DeleteFileOrDirectory(assetPath + ".meta");

                    var progress = (float)i * 1.0f / (float)allAssetsPaths.Count;
                    EditorUtility.DisplayProgressBar("The Visual Engine", "Processing " + Path.GetFileName(assetPath), progress);
                }

                EditorUtility.ClearProgressBar();

                Debug.Log("<b>[The Visual Engine]</b> " + allAssetsPaths.Count.ToString() + " assets have been deleted.");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Cleanup All Name GUID", GUILayout.Height(24)))
            {
                allAssetsPaths = new List<string>();

                GetAssetsPaths("TVE Model", "*.asset");
                GetAssetsPaths("TVE Material", "*.mat");
                GetAssetsPaths("TVE Texture", "*.png");
                GetAssetsPaths("TVE Texture", "*.tga");

                for (int i = 0; i < allAssetsPaths.Count; i++)
                {
                    var assetPath = allAssetsPaths[i];

                    var pathData = TVEUtils.GetPathData(assetPath);

                    AssetDatabase.RenameAsset(assetPath, pathData.name + " (TVE " + pathData.type +  ")");

                    var progress = (float)i * 1.0f / (float)allAssetsPaths.Count;
                    EditorUtility.DisplayProgressBar("The Visual Engine", "Processing " + Path.GetFileName(assetPath), progress);
                }

                EditorUtility.ClearProgressBar();

                Debug.Log("<b>[The Visual Engine]</b> " + allAssetsPaths.Count.ToString() + " assets have been processed.");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Cleanup Model Name GUID", GUILayout.Height(24)))
            {
                allAssetsPaths = new List<string>();

                GetAssetsPaths("TVE Model", "*.asset");

                for (int i = 0; i < allAssetsPaths.Count; i++)
                {
                    var assetPath = allAssetsPaths[i];
                    var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);

                    var pathData = TVEUtils.GetPathData(assetPath);

                    mesh.name = pathData.name + " (TVE Model)";

                    AssetDatabase.RenameAsset(assetPath, mesh.name);

                    var progress = (float)i * 1.0f / (float)allAssetsPaths.Count;
                    EditorUtility.DisplayProgressBar("The Visual Engine", "Processing " + Path.GetFileName(assetPath), progress);
                }

                EditorUtility.ClearProgressBar();

                Debug.Log("<b>[The Visual Engine]</b> " + allAssetsPaths.Count.ToString() + " meshes have been processed.");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Cleanup Material Name GUID", GUILayout.Height(24)))
            {
                allAssetsPaths = new List<string>();

                GetAssetsPaths("TVE Material", "*.mat");

                for (int i = 0; i < allAssetsPaths.Count; i++)
                {
                    var assetPath = allAssetsPaths[i];
                    var material = AssetDatabase.LoadAssetAtPath<Material>(assetPath);

                    var pathData = TVEUtils.GetPathData(assetPath);

                    material.name = pathData.name + " (TVE Material)";

                    AssetDatabase.RenameAsset(assetPath, material.name);

                    var progress = (float)i * 1.0f / (float)allAssetsPaths.Count;
                    EditorUtility.DisplayProgressBar("The Visual Engine", "Processing " + Path.GetFileName(assetPath), progress);
                }

                EditorUtility.ClearProgressBar();

                Debug.Log("<b>[The Visual Engine]</b> " + allAssetsPaths.Count.ToString() + " materials have been processed.");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Cleanup Texture Name GUID", GUILayout.Height(24)))
            {
                allAssetsPaths = new List<string>();

                GetAssetsPaths("TVE Texture", "*.png");
                GetAssetsPaths("TVE Texture", "*.tga");

                for (int i = 0; i < allAssetsPaths.Count; i++)
                {
                    var assetPath = allAssetsPaths[i];

                    var pathData = TVEUtils.GetPathData(assetPath);

                    AssetDatabase.RenameAsset(assetPath, pathData.name + " (TVE Texture)");

                    var progress = (float)i * 1.0f / (float)allAssetsPaths.Count;
                    EditorUtility.DisplayProgressBar("The Visual Engine", "Processing " + Path.GetFileName(assetPath), progress);
                }

                EditorUtility.ClearProgressBar();

                Debug.Log("<b>[The Visual Engine]</b> " + allAssetsPaths.Count.ToString() + " textures have been processed.");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            GUILayout.EndVertical();
            GUILayout.Space(10);
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUILayout.Space(15);
        }

        void SetGUIStyles()
        {
            stylePopup = new GUIStyle(EditorStyles.popup)
            {
                alignment = TextAnchor.MiddleCenter
            };
        }

        void GetAssetsPaths(string searchPattern, string extension)
        {
            if (selectionIndex == 0)
            {
                MethodInfo getActiveFolderPath = typeof(ProjectWindowUtil).GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
                string selectedFolder = (string)getActiveFolderPath.Invoke(null, null);

                var selection = Directory.GetFiles(selectedFolder, extension, SearchOption.AllDirectories);

                for (int i = 0; i < selection.Length; i++)
                {
                    if (selection[i].Contains(searchPattern))
                    {
                        allAssetsPaths.Add(selection[i]);
                    }
                }
            }
            else
            {
                var selection = Selection.objects;

                for (int i = 0; i < selection.Length; i++)
                {
                    var assetPath = AssetDatabase.GetAssetPath(selection[i]);

                    if (assetPath.Contains(searchPattern))
                    {
                        allAssetsPaths.Add(assetPath);
                    }
                }
            }
        }
    }
}


