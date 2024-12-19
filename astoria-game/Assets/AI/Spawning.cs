using UnityEngine;

public class Spawning : MonoBehaviour
{
    [SerializeField] private float outerRadius;
    [SerializeField] private float innerRadius;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject bot;
    private GameObject player;

    private void Start()
    {
        player = this.gameObject;
    }

    void Update()
    {
        Vector3 position = player.transform.position;
        RaycastHit hit;
        Vector2 destinationV2 = Random.insideUnitCircle * outerRadius + new Vector2(innerRadius, innerRadius);
        Physics.Raycast(new Vector3(destinationV2.x, 1000f, destinationV2.y), Vector3.down, out hit, 10000f, layerMask);
        while (hit.collider.gameObject.layer != 16)
        {
            destinationV2 = Random.insideUnitCircle * outerRadius + new Vector2(innerRadius, innerRadius);
            Physics.Raycast(new Vector3(destinationV2.x, 1000, destinationV2.y), Vector3.down, out hit, 10000f, layerMask);
        }
        Instantiate(bot, new Vector3(destinationV2.x, 1000 - hit.distance, destinationV2.y), Quaternion.identity);
    }
}
