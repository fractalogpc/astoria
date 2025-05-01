using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NavMeshDataHandler : MonoBehaviour
{
    [Header("NavMesh Settings")]
    [SerializeField] private float boundsExpansion = 0.05f;
    [SerializeField] private int agentTypeID = 0;

    private NavMeshData navMeshData;
    private NavMeshDataInstance navMeshInstance;

    private void Start() {
        BuildNavMeshForThisObject();
    }

    private void BuildNavMeshForThisObject() {
        var sources = new List<NavMeshBuildSource>();

        // Collect mesh data from this object only
        NavMeshBuilder.CollectSources(transform, LayerMask.GetMask("ConstructionComponent"), NavMeshCollectGeometry.PhysicsColliders, 0, new List<NavMeshBuildMarkup>(), sources);

        // Calculate bounds
        Bounds bounds = CalculateWorldBounds();
        bounds.Expand(boundsExpansion); // Expand slightly for overlap

        // Create NavMeshData
        navMeshData = new NavMeshData(agentTypeID);
        navMeshInstance = NavMesh.AddNavMeshData(navMeshData);

        // Bake asynchronously
        NavMeshBuilder.UpdateNavMeshDataAsync(navMeshData, NavMesh.GetSettingsByID(agentTypeID), sources, bounds);
    }

    private Bounds CalculateWorldBounds() {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            return new Bounds(transform.position, Vector3.one);
        }

        Bounds bounds = renderers[0].bounds;
        foreach (var rend in renderers)
        {
            bounds.Encapsulate(rend.bounds);
        }
        return bounds;
    }

    private void OnDestroy() {
        navMeshInstance.Remove();
    }
}