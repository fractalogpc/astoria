using UnityEngine;
using UnityEngine.InputSystem;

public class MenuLantern : MonoBehaviour
{
    public Light lanternLight;
    public GameObject lanternModel;
    public GameObject wick;

    [SerializeField] private bool _startsOn = false;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        if (_startsOn) ToggleLantern();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == lanternModel)
                {
                    ToggleLantern();
                }
            }
        }
    }

    void ToggleLantern()
    {
        if (lanternLight != null)
        {
            lanternLight.enabled = !lanternLight.enabled;
            wick.SetActive(lanternLight.enabled);
            Object.FindFirstObjectByType<MenuSun>().LanternClick(lanternLight.enabled);
        }
    }
}
