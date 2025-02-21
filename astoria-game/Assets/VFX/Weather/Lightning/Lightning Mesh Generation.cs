using System.Collections.Generic;
using System.Linq;
using SteamAudio;
using UnityEngine;
using Color = UnityEngine.Color;
using Vector3 = UnityEngine.Vector3;

public class LightningMeshGeneration : MonoBehaviour {
    public Transform spawn;
    public Transform goal;

    [SerializeField]
    public int numSegments = 30;
    
    public Vector3[] Vertex = new Vector3[30];

    public List<Vector3> SubVertexDirection;
    public List<Vector3> SubVertexStart;
    public List<Vector3> SubVertexGoal;
    public List<Vector3[]> SubVertex = new List<Vector3[]>();

    [Range(0,1)]
    public float lerp = .75f;
    
    private bool canDebug = false;

    public Mesh mesh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update() {

        if (Input.anyKeyDown)
        {
            SubVertexDirection.Clear();
            SubVertexStart.Clear();
            SubVertexGoal.Clear();
            SubVertex.Clear();
            FormLightning();
            canDebug = true;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FormMesh();
        }
        

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
            if (i > 0)
            {
                Debug.DrawRay(Vertex[i - 1], Vertex[i] - Vertex[i - 1], new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
            }
            if (i == numSegments - 1)
            {
                Debug.DrawRay(Vertex[i], goal.position - Vertex[i], new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
            }

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
            SubVertexGoal.Add(SubVertexStart[i] + Random.insideUnitSphere * Vector3.Distance(goal.position, SubVertexStart[i]));
            for (int j = 0; j < 8; j++)
            {
                //Random.seed = Mathf.FloorToInt(SubVertexStart[i].x + SubVertexStart[i].y + SubVertexStart[i].z * 20 + i) + 50 * j + 100;// + Time.frameCount;
                if (j == 0)
                {
                    SubVertex[i][j] = SubVertexStart[i];
                }
                else
                {
                    Vector3 point = Vector3.Lerp(SubVertexStart[i], SubVertexGoal[i], (float)j / 8) + Vector3.Lerp(Random.onUnitSphere, Vector3.Normalize(SubVertexGoal[i] - SubVertexStart[i]), lerp) * Mathf.Clamp(Vector3.Distance(goal.position, SubVertexStart[i]) / 5f, 1f, 100f);
                    SubVertex[i][j] = point; // Random.insideUnitSphere + line + SubVertexStart[i];
                }
                
            }
        }
    }

    void FormMesh() {
        mesh = new Mesh {
            name = "Procedural Mesh"
        };
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int i = 0; i < Vertex.Length; i++)
        {
            Vector3 nextVertex = i < Vertex.Length - 1 ? Vertex[i + 1] : goal.position;
            Vector3 Tangent = nextVertex - Vertex[i];
            Vector3 BiNormal = Vector3.Normalize(Vector3.Cross(Tangent, Tangent + new Vector3(1231, 12f, -1203f)));
            Vector3 RotNormal = Quaternion.AngleAxis(90, Tangent) * BiNormal;

            vertices.Add(Vertex[i] + BiNormal);
            vertices.Add(Vertex[i] + RotNormal);
            vertices.Add(Vertex[i] - BiNormal);
            vertices.Add(Vertex[i] - RotNormal);
        }

        vertices.Add(goal.position + new Vector3(1, 0, 0));
        vertices.Add(goal.position + new Vector3(0, 0, 1));
        vertices.Add(goal.position + new Vector3(-1, 0, 0));
        vertices.Add(goal.position + new Vector3(0, 0, -1));

        for (int i = 0; i < vertices.Count - 5; i++)
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

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        
        GetComponent<MeshFilter>().mesh = mesh;
    }
    
}
