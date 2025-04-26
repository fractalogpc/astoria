using Unity.Mathematics;
using UnityEngine;

public class TransferCameraRotationToTornadoShader : MonoBehaviour
{
    [SerializeField] private Material tornado;
    [SerializeField] private Camera camera;

    // Update is called once per frame
    void Update()
    {
        tornado.SetVector("_CameraRotation", camera.transform.rotation.eulerAngles);
    }
}
