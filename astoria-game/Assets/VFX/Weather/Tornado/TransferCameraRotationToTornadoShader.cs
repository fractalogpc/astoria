using Unity.Mathematics;
using UnityEngine;

public class TransferCameraRotationToTornadoShader : MonoBehaviour
{
    [SerializeField] private Material tornado;
    [SerializeField] private Camera camera;
    [SerializeField] private Light sun;


    // Update is called once per frame
    void Update()
    {
        if (sun == null) {
            sun = RenderSettings.sun;
        }

        tornado.SetVector("_CameraRotation", Mathf.Deg2Rad * camera.transform.rotation.eulerAngles);
        tornado.SetVector("_Resolution", new Vector2(Screen.width, Screen.height));
        tornado.SetVector("_MainLightDirection", sun.transform.forward);
        tornado.SetVector("_MainLightColor", sun.color);
    }
}
