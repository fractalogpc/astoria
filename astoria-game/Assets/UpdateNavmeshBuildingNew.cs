using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections;
using System.Collections.Generic;

public class UpdateNavmeshBuildingNew : MonoBehaviour
{
    NavMeshSurface surface;
    int childCount;
    void Start() {
        surface = GetComponent<NavMeshSurface>();
        childCount = transform.childCount;
    }
    void Update() {
        if (childCount != transform.childCount) {
            // var sources = new List<NavMeshBuildSource>();
            // var markups = new List<NavMeshBuildMarkup>();

            // NavMeshBuilder.CollectSources(transform, surface.layerMask, NavMeshCollectGeometry.PhysicsColliders, surface.defaultArea, markups, sources);

            // // Define bounds based on collected sources
            // var bounds = NavMeshBuilder.CalculateWorldBounds(sources);

            // NavMeshBuilder.UpdateNavMeshDataAsync(surface.navMeshData, surface.GetBuildSettings(), sources, bounds);
        }
        foreach (Transform child in transform) {
            if (child.gameObject.tag == "Foundation" && !child.gameObject.TryGetComponent<NavMeshLink>(out NavMeshLink link)) {
                AddNavMeshLinks(child.gameObject);
            }
        }
    }

    private void AddNavMeshLinks(GameObject foundation) {
        for (int i = 0; i < 4; i++) {
            NavMeshLink tempLink = foundation.AddComponent<NavMeshLink>();
            tempLink.startPoint = new Vector3(Mathf.Cos(Mathf.PI/2 * i) * 1.48f, 0, Mathf.Sin(Mathf.PI/2 * i) * 1.48f);
            tempLink.endPoint = new Vector3(Mathf.Cos(Mathf.PI/2 * i) * 1.53f, 0, Mathf.Sin(Mathf.PI/2 * i) * 1.53f);
            tempLink.width = 3;
        }
    }
}