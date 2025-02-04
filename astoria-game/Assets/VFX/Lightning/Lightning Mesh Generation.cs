using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class LightningMeshGeneration : MonoBehaviour {
    public Transform spawn;
    public Transform goal;

    public Vector3[] Bitangent = new Vector3[3];
    public Vector3[] Binormal = new Vector3[3];
    public Vector3[] Biongo = new Vector3[3];
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
        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
            {
                
            }
            else
            {
                
            }
        }
    }
}
