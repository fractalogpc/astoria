using UnityEngine;

public class WiperTracker : MonoBehaviour
{

    [SerializeField] private Transform _wiperPoint1;
    [SerializeField] private Transform _wiperPoint2;
    [SerializeField] private Material _windshieldMaterial;

    void Update()
    {
        _windshieldMaterial.SetVector("_WiperPoint1", _wiperPoint1.position);
        _windshieldMaterial.SetVector("_WiperPoint2", _wiperPoint2.position);
    }

}
