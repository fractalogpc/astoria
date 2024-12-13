using UnityEngine;

public class SetLayer : MonoBehaviour
{
    
    public void SetLayers(int layer){
        var children = transform.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children) {
            child.gameObject.layer = layer;
        }
    }

}
