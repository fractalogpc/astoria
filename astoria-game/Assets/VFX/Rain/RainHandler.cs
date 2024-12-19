using UnityEngine;
using UnityEngine.VFX;

public class RainHandler : MonoBehaviour
{
    [SerializeField]
    private VisualEffect rain;
    [SerializeField]
    private GameObject player;
    public bool rainToggle;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;

        if (rainToggle) {
            rain.enabled = true;
        } else {
            rain.enabled = false;
        }

    }
}
