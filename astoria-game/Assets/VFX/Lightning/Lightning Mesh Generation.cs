using System.Collections.Generic;
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
    public List<Vector3[]> SubVertex = new List<Vector3[]>(new Vector3[15][]);
    [SerializeField] private List<int[]> listOfArrays;

    private bool canDebug = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        
        /*Bitangent[0] = goal.position - spawn.position;
        Binormal[0] = new Vector3(Bitangent[0].x, Bitangent[0].z, -Bitangent[0].y) * Mathf.Sign(Bitangent[0].z);
        Biongo[0] = (Vector3.Normalize(Bitangent[0]) + Vector3.Normalize(Binormal[0])) * Vector3.Dot(Vector3.Normalize(Bitangent[0]), Vector3.down);
        Debug.DrawRay(spawn.position, Vector3.down, Color.black);
        Debug.DrawRay(spawn.position, Bitangent[0], Color.red);
        Debug.DrawRay(spawn.position, Binormal[0], Color.blue);
        Debug.DrawRay(spawn.position, Biongo[0], Color.green);
        for (int i = 1; i < 2; i++)
        {
            Bitangent[i] = goal.position - Biongo[i - 1];
            Binormal[i] = new Vector3(Bitangent[i].x, Bitangent[i].z, -Bitangent[i].y) * Mathf.Sign(Bitangent[i].z);
            Biongo[i] = (Vector3.Normalize(Bitangent[i]) + Vector3.Normalize(Binormal[i])) * Vector3.Dot(Vector3.Normalize(Bitangent[i]), Vector3.down);
            Debug.DrawRay(Biongo[i - 1], Vector3.down, Color.black);
            Debug.DrawRay(Biongo[i - 1], Bitangent[i], Color.red);
            Debug.DrawRay(Biongo[i - 1], Binormal[i], Color.blue);
            Debug.DrawRay(Biongo[i - 1], Biongo[i], Color.green);
        }*/
        
        
        /*for (int i = 0; i < numSegments; i++)
        {
            Random.seed = (int)Mathf.Round(Biongo[i].x + Biongo[i].y + Biongo[i].z) + 1000;
            if (i == 0)
            {
                Bitangent[i] = goal.position - spawn.position;
                Binormal[i] = new Vector3(Bitangent[i].x, Bitangent[i].z, -Bitangent[i].y) * Mathf.Sign(Bitangent[i].z);
                Biongo[i + 1] = (spawn.position + (Vector3.Normalize(Bitangent[i]) + Vector3.Normalize(Binormal[i])) + Random.onUnitSphere * .2f);
                //Debug.DrawRay(spawn.position, Vector3.down, Color.black);
                //Debug.DrawRay(spawn.position, Bitangent[i], Color.red);
                //Debug.DrawRay(spawn.position, Binormal[i], Color.blue);
                Debug.DrawRay(spawn.position, Biongo[i + 1] - spawn.position, Color.green);
            }
            else
            {
                Bitangent[i] = goal.position - Biongo[i];
                Binormal[i] = new Vector3(Bitangent[i].x, Bitangent[i].z, -Bitangent[i].y) * Mathf.Sign(Bitangent[i].z);
                Biongo[i + 1] = (Biongo[i] + (Vector3.Normalize(Bitangent[i]) + Vector3.Normalize(Binormal[i])) + Random.onUnitSphere * .2f);
                //Debug.DrawRay(Biongo[i], Vector3.down, Color.black);
                //Debug.DrawRay(Biongo[i], Bitangent[i], Color.red);
                //Debug.DrawRay(Biongo[i], Binormal[i], Color.blue);
                Debug.DrawRay(Biongo[i], Biongo[i + 1] - Biongo[i], Color.green);
            }

            if (i == numSegments)
            {
                Debug.DrawRay(Biongo[i], goal.position, Color.green);
            }
        }*/

        if (Input.anyKeyDown)
        {
            SubVertexDirection.Clear();
            SubVertexStart.Clear();
            SubVertexGoal.Clear();
            SubVertex.Clear();
            FormLightning();
            canDebug = true;
        }

        if (canDebug)
        {
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
            
            for (int i = 0; i < SubVertexDirection.Count; i++)
            {
                Debug.DrawRay(SubVertexStart[i], SubVertexDirection[i], Color.magenta);
                Debug.DrawRay(SubVertexStart[i], -SubVertexDirection[i], Color.magenta);
                //Debug.DrawRay(SubVertexStart[i], SubVertexGoal[i], new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                for (int j = 1; j < 15; j++)
                {
                    Debug.DrawRay(SubVertex[0][0], SubVertex[0][1] - SubVertex[0][0], new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                    Debug.DrawRay(SubVertex[1][0], SubVertex[1][1] - SubVertex[1][0], new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                }
                /*for (int j = 1; j < 15; j++)
                {
                    Debug.DrawRay(SubVertex[i][j - 1], SubVertex[i][j] - SubVertex[i][j - 1], new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
        
                    if (j == 14)
                    {
                        Debug.DrawRay(SubVertex[i][j], SubVertexGoal[i] - SubVertex[i][j], new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                    }
                }*/
            }
        }
        
    }

    void FormLightning() {
        for (int i = 0; i < numSegments; i++)
        {
            Random.seed = Mathf.FloorToInt(spawn.position.x * 20 + spawn.position.y * 20 + spawn.position.z * 20 + i) + 100 * i + Time.frameCount;
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

            if (i > 1)
            {
                Vector3 tangent = goal.position - Vertex[i];
                Vector3 biNormal = Vector3.Normalize(Vector3.Cross(Vertex[i] - Vertex[i - 1], tangent + new Vector3(100, -12, 402)));
                if (Random.Range(0f, 1f) > .5)
                {
                    SubVertexDirection.Add(biNormal * 2f);
                    SubVertexStart.Add(Vertex[i]);
                    SubVertex.Add(new Vector3[15]);
                }
                Debug.DrawRay(Vertex[i], tangent, Color.red);
                Debug.DrawRay(Vertex[i] - ((Vertex[i] - Vertex[i - 1]) * .5f), biNormal, Color.magenta);
            }

            
        }
        
        for (int i = 0; i < SubVertexDirection.Count; i++)
        {
            SubVertexGoal.Add(SubVertexStart[i] + Random.insideUnitSphere * 100);
            for (int j = 0; j < 15; j++)
            {
                Random.seed = Mathf.FloorToInt(SubVertexStart[i].x + SubVertexStart[i].y + SubVertexStart[i].z * 20 + i) + 50 * i + Time.frameCount;
                Vector3 line = Random.insideUnitSphere * Mathf.Clamp(Vector3.Distance(Vector3.Lerp(SubVertexStart[i], SubVertexGoal[i], (float)j / numSegments), SubVertexGoal[i]), .1f, 10);
                SubVertex[i][j] = Vector3.Lerp(SubVertexStart[i], SubVertexGoal[i], (float)i / numSegments) + new Vector3(line.x, Mathf.Abs(line.y), line.z);
            }
        }
    }
}
