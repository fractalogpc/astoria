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

    public List<Vector3> SubVertex;
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

        for (int i = 0; i < numSegments; i++)
        {
            Random.seed = Mathf.FloorToInt(spawn.position.x * 20 + spawn.position.y * 20 + spawn.position.z * 20 + i) + 100 * i;
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
            
            //if (Random.Range(0, 1))
        }
    }
}
