using Unity.AI.Navigation;
using UnityEngine;

public class UpdateNavmeshBuilding : MonoBehaviour
{
    private int childCount = 0;
    private NavMeshSurface navMeshSurface;
    void Start() {
        navMeshSurface = GetComponent<NavMeshSurface>();
        childCount = transform.childCount;
        navMeshSurface.BuildNavMesh();
    }
    void Update() {
        if (childCount != transform.childCount) {
            childCount = transform.childCount;
            navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
        }
    }
}