using UnityEngine;
using Mirror;

public class CoreController : NetworkBehaviour
{

    public float height;
    public Vector3 position;
    public Quaternion rotation;
    public float scale;

    private void OnEnable()
    {
        if (NetworkClient.localPlayer.GetComponentInChildren<ConstructionCore>().Core != null)
        {
            Debug.LogError("Core already exists");
            Destroy(gameObject);
            return;
        }

        position = new Vector3(transform.position.x, height, transform.position.z);
        rotation = transform.rotation;
        scale = transform.localScale.x;

        NetworkClient.localPlayer.GetComponentInChildren<ConstructionCore>().Core = this;
    }
}
