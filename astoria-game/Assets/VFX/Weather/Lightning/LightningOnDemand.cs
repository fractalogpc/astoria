using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LightningOnDemand : MonoBehaviour
{

    [SerializeField] private Transform spawn;
    [SerializeField] private Transform goal;

    [SerializeField] private int numSegments = 30;
    [SerializeField] private float _hitTime = 0.3f;
    [SerializeField] private float lightningSize = .1f;
    [SerializeField] private Material lightningMaterial;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float downStrength = 1f;
    [SerializeField] private float randomStrength = 5f;
    [SerializeField] private float SubVertexMaxSize = 1f;
    [SerializeField] private float SubVertexRandomizationStrength = 1f;
    [Range(0, 1)] public float lerp = .75f;


    public List<Vector3> SubVertexDirection;
    public List<Vector3> SubVertexStart;
    public List<Vector3> SubVertexGoal;
    public List<Vector3[]> SubVertex = new List<Vector3[]>();
    public Vector3[] Vertex = new Vector3[30];


    private bool _lightningIsSpawned = false;
    private float _hitTimer;
    private Mesh mesh;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        SubVertexDirection = new List<Vector3>();
        SubVertexStart = new List<Vector3>();
        SubVertexGoal = new List<Vector3>();
    }

    public void SpawnLightning()
    {
        if (_lightningIsSpawned) return;

        SpawnLightningMesh();

        _lightningIsSpawned = true;
        _hitTimer = _hitTime;

    }

    private void Update() {
        if (_lightningIsSpawned)
        {
            _hitTimer -= Time.deltaTime;
            float brightness = animCurve.Evaluate(1 - _hitTimer / _hitTime);
            if (_hitTimer < 0)
            {
                _lightningIsSpawned = false;
                ClearLightning();
            }
            lightningMaterial.SetFloat("_Brightness", brightness);
        }
    }

    private void SpawnLightningMesh()
    {
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

        lightningMaterial.SetFloat("_Brightness", 0);
    }

    private void ClearLightning() {
        mesh.Clear();
    }

    void FormLightning() {
        //main path
        for (int i = 0; i < numSegments; i++)
        {
            Vector3 line = Random.onUnitSphere * Mathf.Clamp(Vector3.Distance(Vector3.Lerp(spawn.position, goal.position, (float)i / numSegments), goal.position), .1f, randomStrength);
            Vertex[i] = Vector3.Lerp(spawn.position, goal.position, (float)i / numSegments) + new Vector3(line.x, Mathf.Abs(line.y), line.z);
            if (i == numSegments - 1) Vertex[i] = goal.position;
            if (i == 0) Vertex[i] = spawn.position;
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
            randomPos = Vector3.Lerp(randomPos, Vector3.Normalize(goal.position - spawn.position), downStrength);
            SubVertexGoal.Add(SubVertexStart[i] + randomPos * Vector3.Distance(goal.position, SubVertexStart[i]));
            for (int j = 0; j < 8; j++)
            {
                if (j == 0)
                {
                    SubVertex[i][j] = SubVertexStart[i];
                }
                else
                {
                    Vector3 randomDirection = Random.onUnitSphere * Mathf.Clamp(Vector3.Distance(goal.position, SubVertexStart[i]) / 5f, 1f, SubVertexMaxSize) * SubVertexRandomizationStrength;
                    randomDirection.y = -Mathf.Abs(randomDirection.y);
                    Vector3 point = Vector3.Lerp(SubVertexStart[i], SubVertexGoal[i], (float)j / 8) + Vector3.Lerp(randomDirection, Vector3.Normalize(SubVertexGoal[i] - SubVertexStart[i]), lerp) * Mathf.Clamp(Vector3.Distance(goal.position, SubVertexStart[i]) / 5f, 1f, SubVertexMaxSize);
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

            vertices.Add((Vectors[i] - spawn.position) + BiNormal * ((Vectors.Length - i) * lightningSize));
            vertices.Add((Vectors[i] - spawn.position) + RotNormal * ((Vectors.Length - i) * lightningSize));
            vertices.Add((Vectors[i] - spawn.position) - BiNormal * ((Vectors.Length - i) * lightningSize));
            vertices.Add((Vectors[i] - spawn.position) - RotNormal * ((Vectors.Length - i) * lightningSize));
        }

        if (offset < 1)
        {
            vertices.Add((goal.position - spawn.position) + new Vector3(1f, 0, 0));
            vertices.Add((goal.position - spawn.position) + new Vector3(0, 0, 1f));
            vertices.Add((goal.position - spawn.position) + new Vector3(-1f, 0, 0));
            vertices.Add((goal.position - spawn.position) + new Vector3(0, 0, -1f));
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