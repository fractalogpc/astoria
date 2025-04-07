using UnityEngine;

public class TreeDestroyOnCollideTrigger : MonoBehaviour
{

    private void Update() {
        // Check if there are any trees in the radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, LayerMask.GetMask("Tree"));
        if (colliders.Length > 0) {
            // Destroy the trees in the radius
            foreach (Collider collider in colliders) {
                GameObject tree = TreeChopping.Instance.RealizeTree(collider.transform.position);
                tree.GetComponent<TreeChoppable>().DamageFromEnvironment(1000f, collider.transform.position);
            }
        }
    }

}
