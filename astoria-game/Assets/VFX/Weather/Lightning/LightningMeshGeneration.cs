using System.Collections.Generic;
using System.Linq;
using SteamAudio;
using UnityEngine;
using Color = UnityEngine.Color;
using Vector3 = UnityEngine.Vector3;

public class LightningMeshGeneration : MonoBehaviour {
    public Transform spawn;
    public GameObject light;
    public Transform goal;

    [SerializeField] public int numSegments = 30;
    [SerializeField] private float meanSpawnTime = 1f;
    [SerializeField] private float spawnTimeVariance = .5f;
    [SerializeField] private float spawnHeight = 10f;
    [SerializeField] private Transform spawnPositionCenter;
    [SerializeField] private float spawnPositionRadius;
    public Vector3 spawnPosition;
    public Vector3 goalPosition;
    public float downStrength;

    public Vector3[] Vertex = new Vector3[30];

    public List<Vector3> SubVertexDirection;
    public List<Vector3> SubVertexStart;
    public List<Vector3> SubVertexGoal;
    public List<Vector3[]> SubVertex = new List<Vector3[]>();

    [Range(0, 1)] public float lerp = .75f;

    private bool canDebug = false;

    private bool spawning = true;
    private float spawnTimer = 0f;

    public Mesh mesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    

    // Update is called once per frame
    void Update() {
        if (spawning)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                spawnTimer = meanSpawnTime + Random.Range(-spawnTimeVariance, spawnTimeVariance);
                spawnPosition = spawnPositionCenter.position + Random.insideUnitSphere * spawnPositionRadius;
                spawnPosition.y = spawnHeight;

                spawn.position = spawnPosition;

                RaycastHit hit;
                if (Physics.Raycast(spawnPositionCenter.position + Random.onUnitSphere * spawnPositionRadius, Vector3.down, out hit))
                {
                    goal.position = hit.point;
                    goalPosition = goal.position;
                }
                else return;

                SpawnLightning();
            }
        }

        /*switch (spawnTimer)
        {
            case float t when t > 0:
                spawnTimer -= Time.deltaTime;
                break;
            default:
                print("hi");
                break;
        }*/

        // if (Input.anyKeyDown)
        // {
        //     SubVertexDirection.Clear();
        //     SubVertexStart.Clear();
        //     SubVertexGoal.Clear();
        //     SubVertex.Clear();
        //     mesh.Clear();
        //     FormLightning();
        //     canDebug = true;
        // }

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     FormMesh(Vertex);
        //     for (int i = 0; i < SubVertex.Count; i++)
        //     {
        //         FormMesh(SubVertex[i]);
        //     }
        //     light.SetActive(true);
        // }

    }
    
    public void ActivateLightning() {
        spawning = true;
    }

    public void DeactivateLightning() {
        spawning = false;
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
        light.SetActive(true);

        // Random duration
        float duration = Random.value + 1f;
        Invoke("ClearLightning", duration);
    }

    private void ClearLightning() {
        light.SetActive(false);
        mesh.Clear();
    }

    void FormLightning() {
        //main path
        for (int i = 0; i < numSegments; i++)
        {
            Vector3 line = Random.onUnitSphere * Mathf.Clamp(Vector3.Distance(Vector3.Lerp(spawn.position, goal.position, (float)i / numSegments), goal.position), .1f, 50);
            Vertex[i] = Vector3.Lerp(spawn.position, goal.position, (float)i / numSegments) + new Vector3(line.x, Mathf.Abs(line.y), line.z);
            //create sub-branch
            if (i > 1)
            {
                Vector3 tangent = goal.position - Vertex[i];
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
            randomPos = Vector3.Lerp(randomPos, Vector3.down, downStrength);
            SubVertexGoal.Add(SubVertexStart[i] + randomPos * Vector3.Distance(goal.position, SubVertexStart[i]));
            for (int j = 0; j < 8; j++)
            {
                if (j == 0)
                {
                    SubVertex[i][j] = SubVertexStart[i];
                }
                else
                {
                    Vector3 randomDirection = Random.onUnitSphere;
                    randomDirection.y = -Mathf.Abs(randomDirection.y);
                    Vector3 point = Vector3.Lerp(SubVertexStart[i], SubVertexGoal[i], (float)j / 8) + Vector3.Lerp(randomDirection, Vector3.Normalize(SubVertexGoal[i] - SubVertexStart[i]), lerp) * Mathf.Clamp(Vector3.Distance(goal.position, SubVertexStart[i]) / 5f, 1f, 100f);
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
            Vector3 nextVertex = i < Vectors.Length - 1 ? Vectors[i + 1] : goal.position;
            Vector3 Tangent = nextVertex - Vectors[i];
            Vector3 BiNormal = Vector3.Normalize(Vector3.Cross(Tangent, Tangent + new Vector3(1231, 12f, -1203f)));
            Vector3 RotNormal = Quaternion.AngleAxis(90, Tangent) * BiNormal;

            vertices.Add((Vectors[i] - spawn.position) + BiNormal);
            vertices.Add((Vectors[i] - spawn.position) + RotNormal);
            vertices.Add((Vectors[i] - spawn.position) - BiNormal);
            vertices.Add((Vectors[i] - spawn.position) - RotNormal);
        }

        if (offset < 1)
        {
            vertices.Add((goal.position - spawn.position) + new Vector3(.1f, 0, 0));
            vertices.Add((goal.position - spawn.position) + new Vector3(0, 0, .1f));
            vertices.Add((goal.position - spawn.position) + new Vector3(-.1f, 0, 0));
            vertices.Add((goal.position - spawn.position) + new Vector3(0, 0, -.1f));
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