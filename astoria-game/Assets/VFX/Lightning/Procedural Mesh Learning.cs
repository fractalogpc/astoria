using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Mathematics;

using static Unity.Mathematics.math;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralMeshLearning : MonoBehaviour
{
    private void OnEnable() {
        int vertexAttributeCount = 4;
        int vertexCount = 4;
        int triangleIdexCount = 6;
        
        Mesh.MeshDataArray meshDataArry = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData = meshDataArry[0];
        
        var vertexAttributes = new NativeArray<VertexAttributeDescriptor>(vertexAttributeCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

        vertexAttributes[0] = new VertexAttributeDescriptor(dimension: 3);
        vertexAttributes[1] = new VertexAttributeDescriptor(VertexAttribute.Normal, dimension: 3, stream: 1);
        vertexAttributes[2] = new VertexAttributeDescriptor(VertexAttribute.Tangent, dimension: 4, stream: 1);
        vertexAttributes[3] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, dimension: 2, stream: 3);
        
        meshData.SetVertexBufferParams(vertexCount, vertexAttributes);
        vertexAttributes.Dispose();

        NativeArray<float3> positions = meshData.GetVertexData<float3>();
        positions[0] = 0f;
        positions[1] = right();
        positions[2] = up();
        positions[3] = float3(1f, 1f, 0f);

        NativeArray<float3> normals = meshData.GetVertexData<float3>(1);
        normals[0] = normals[1] = normals[2] = normals[3] = back();
        
        NativeArray<float4> tangents = meshData.GetVertexData<float4>(2);
        tangents[0] = tangents[1] = tangents[2] = tangents[3] = float4(1f, 0f, 0f, -1f);
        
        NativeArray<float2> uv = meshData.GetVertexData<float2>(3);
        uv[0] = 0f;
        uv[1] = float2(1f, 0f);
        uv[2] = float2(0f, 1f);
        uv[3] = 1f;
        
        meshData.SetIndexBufferParams(triangleIdexCount, IndexFormat.UInt16);
        NativeArray<ushort> triangleIndices = meshData.GetIndexData<ushort>();
        triangleIndices[0] = 0;
        triangleIndices[1] = 2;
        triangleIndices[2] = 1;
        triangleIndices[3] = 1;
        triangleIndices[4] = 2;
        triangleIndices[5] = 3;

        meshData.subMeshCount = 1;
        meshData.SetSubMesh(0, new SubMeshDescriptor(0, triangleIdexCount));
        
        var mesh = new Mesh {
            name = "Procedural Mesh"
        };

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArry, mesh);

        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
