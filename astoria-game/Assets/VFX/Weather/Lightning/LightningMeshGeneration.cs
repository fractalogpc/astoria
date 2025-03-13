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

    [SerializeField]
    public int numSegments = 30;
    [SerializeField] private float meanSpawnTime = 1f;
    [SerializeField] private float spawnTimeVariance = .5f;
    [SerializeField] private float spawnHeight = 10f;
    [SerializeField] private Transform spawnPositionCenter;
    [SerializeField] private float spawnPositionRadius;
    
    public Vector3[] Vertex = new Vector3[30];

    public List<Vector3> SubVertexDirection;
    public List<Vector3> SubVertexStart;
    public List<Vector3> SubVertexGoal;
    public List<Vector3[]> SubVertex = new List<Vector3[]>();

    [Range(0,1)]
    public float lerp = .75f;
    
    private bool canDebug = false;

    private bool spawning = true;
    private float spawnTimer = 0f;

    public Mesh mesh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void ActivateLightning() {
        spawning = true;
    }

    public void DeactivateLightning() {
        spawning = false;
    }

    private void SpawnLightning() {
        Debug.Log("Spawning lightning");

        SubVertexDirection.Clear();
        SubVertexStart.Clear();
        SubVertexGoal.Clear();
        SubVertex.Clear();
        mesh.Clear();
        FormLightning();
        FormMesh(Vertex);
        // for (int i = 0; i < SubVertex.Count; i++)
        // {
        //     FormMesh(SubVertex[i]);
        // }
        light.SetActive(true);

        // Random duration
        float duration = Random.value + 1f;
        Invoke("ClearLightning", duration);
    }

    private void ClearLightning() {
        light.SetActive(false);
        mesh.Clear();
    }

    // Update is called once per frame
    void Update() {
        if (spawning) {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0) {
                spawnTimer = meanSpawnTime + Random.Range(-spawnTimeVariance, spawnTimeVariance);
                Vector3 spawnPosition = spawnPositionCenter.position + Random.insideUnitSphere * spawnPositionRadius;
                spawnPosition.y = spawnHeight;

                spawn.position = spawnPosition;

                RaycastHit hit;
                if (Physics.Raycast(spawn.position, Vector3.down, out hit)) {
                    goal.position = hit.point;
                } else return;

                SpawnLightning();
            }
        }

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
        

        if (canDebug)
        {
            //main branch
            for (int i = 0; i < numSegments; i++)
            {
                if (i > 0)
                {
                    Debug.DrawRay(Vertex[i - 1], Vertex[i] - Vertex[i - 1], new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                }

                if (i == numSegments - 1)
                {
                    Debug.DrawRay(Vertex[i], goal.position - Vertex[i], new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                }
            }
            
            //sub-branch
            for (int i = 0; i < SubVertexDirection.Count; i++)
            {
                //Debug.DrawRay(SubVertexStart[i], SubVertexGoal[i], new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                for (int j = 1; j < 8; j++)
                {
                    Debug.DrawRay(SubVertex[i][j], SubVertex[i][j - 1] - SubVertex[i][j], new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                }
            }
        }
        
    }

    void FormLightning() {
        //main path
        for (int i = 0; i < numSegments; i++)
        {
            //Random.seed = Mathf.FloorToInt(spawn.position.x * 20 + spawn.position.y * 20 + spawn.position.z * 20 + i) + 100 * i;// + Time.frameCount;
            Vector3 line = Random.onUnitSphere * Mathf.Clamp(Vector3.Distance(Vector3.Lerp(spawn.position, goal.position, (float)i / numSegments), goal.position), .1f, 10);
            Vertex[i] = Vector3.Lerp(spawn.position, goal.position, (float)i / numSegments) + new Vector3(line.x, Mathf.Abs(line.y), line.z);
            //Debug.DrawRay(Vertex[i], (goal.position - spawn.position) * 1 / numSegments, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
            // if (i > 0)
            // {
            //     Debug.DrawRay(Vertex[i - 1], Vertex[i] - Vertex[i - 1], new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
            // }
            // if (i == numSegments - 1)
            // {
            //     Debug.DrawRay(Vertex[i], goal.position - Vertex[i], new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
            // }

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
            SubVertexGoal.Add(SubVertexStart[i] + randomPos * Vector3.Distance(goal.position, SubVertexStart[i]));
            for (int j = 0; j < 8; j++)
            {
                //Random.seed = Mathf.FloorToInt(SubVertexStart[i].x + SubVertexStart[i].y + SubVertexStart[i].z * 20 + i) + 50 * j + 100;// + Time.frameCount;
                if (j == 0)
                {
                    SubVertex[i][j] = SubVertexStart[i];
                }
                else
                {
                    Vector3 randomDirection = Random.onUnitSphere;
                    randomDirection.y = -Mathf.Abs(randomDirection.y);
                    Vector3 point = Vector3.Lerp(SubVertexStart[i], SubVertexGoal[i], (float)j / 8) + Vector3.Lerp(randomDirection, Vector3.Normalize(SubVertexGoal[i] - SubVertexStart[i]), lerp) * Mathf.Clamp(Vector3.Distance(goal.position, SubVertexStart[i]) / 5f, 1f, 100f);
                    SubVertex[i][j] = point; // Random.insideUnitSphere + line + SubVertexStart[i];
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
        
        
        
        /*bad, evil, no good code.
        for (int i = 0; i < SubVertexStart.Count; i++)
        {
            for (int j = 0; j < vertices.Count; i++)
            {
                if (j % 4 == 3)
                {
                    triangles.Add(j);
                    triangles.Add(j + 4);
                    triangles.Add(j + 1);
                    triangles.Add(j);
                    triangles.Add(j + 1);
                    triangles.Add(j - 3);
                }
                else
                {
                    triangles.Add(j);
                    triangles.Add(j + 4);
                    triangles.Add(j + 5);
                    triangles.Add(j);
                    triangles.Add(j + 5);
                    triangles.Add(j + 1);    
                }
            }
            
            for (int j = 0; j < SubVertex[i].Length; j++)
           {
               Vector3 nextVertex = i < SubVertex[i].Length - 1 ? SubVertex[i][j + 1] : SubVertexGoal[i];
               Vector3 Tangent = nextVertex - SubVertex[i][j];
               Vector3 BiNormal = Vector3.Normalize(Vector3.Cross(Tangent, Tangent + new Vector3(1231, 12f, -1203f)));
               Vector3 RotNormal = Quaternion.AngleAxis(90, Tangent) * BiNormal;

               vertices.Add((SubVertex[i][j] - spawn.position) + BiNormal);
               vertices.Add((SubVertex[i][j] - spawn.position) + RotNormal);
               vertices.Add((SubVertex[i][j] - spawn.position) - BiNormal);
               vertices.Add((SubVertex[i][j] - spawn.position) - RotNormal);
           }
        }
        */

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

    void FormSubMesh(int index) {
        //subvertex[index] is the vertex set I'm working with
        //set up a list of the vertices
        //run through SubVertex[index][i] and add them to the list like I do in FormMesh()
        //then generate the triangles
        //then send those to the mesh
        //we have to start at Vertices.Count
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        
        for (int i = 0; i < SubVertex[index].Length; i++)
        {
            Vector3 nextVertex = i < SubVertex[index].Length - 1 ? SubVertex[index][i + 1] : SubVertexGoal[index];
            Vector3 Tangent = nextVertex - SubVertex[index][i];
            Vector3 BiNormal = Vector3.Normalize(Vector3.Cross(Tangent, Tangent + new Vector3(1231, 12f, -1203f)));
            Vector3 RotNormal = Quaternion.AngleAxis(90, Tangent) * BiNormal;

            vertices.Add((SubVertex[index][i] - spawn.position) + BiNormal);
            vertices.Add((SubVertex[index][i] - spawn.position) + RotNormal);
            vertices.Add((SubVertex[index][i] - spawn.position) - BiNormal);
            vertices.Add((SubVertex[index][i] - spawn.position) - RotNormal);
        }

        for (int i = vertices.Count + 1; i < (SubVertex[index].Length - 5); i++)
        {
            if (i % 4 == 3)
            {
                triangles.Add(i);
                triangles.Add(i + 4);
                triangles.Add(i + 1);
                triangles.Add(i);
                triangles.Add(i + 1);
                triangles.Add(i - 3);
            }
            else
            {
                triangles.Add(i);
                triangles.Add(i + 4);
                triangles.Add(i + 5);
                triangles.Add(i);
                triangles.Add(i + 5);
                triangles.Add(i + 1);    
            }
        }

        
        
        GetComponent<MeshFilter>().mesh = mesh;
    }
    
}
