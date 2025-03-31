using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class MapManager : InputHandlerBase
{

    [SerializeField] private RectTransform _mapRectTransform;
    [SerializeField] private Transform _mapMarkerContainer;
    [SerializeField] private GameObject _mapMarkerPrefab;

    [Header("Map Settings")]
    [SerializeField] private float _zoomSpeed = 0.1f;
    [SerializeField] private float _minZoom = 0.5f;
    [SerializeField] private float _maxZoom = 2.0f;
    [SerializeField] private float _startZoom = 1.0f;
    [SerializeField] private float _panSpeed = 0.1f;
    [SerializeField] private Vector2 _maxPanOffset = new Vector2(1000, 1000); // Maximum pan offset in pixels
    [SerializeField] private Vector2 _worldSize = new Vector2(5000, 5000); // Size of the world in meters
    [SerializeField] private Vector2 _mapSize = new Vector2(2000, 2000); // Size of the map in pixels

    private float _currentZoom = 1.0f;
    private bool _isPanning = false;
    private Vector2 _lastMousePosition;

    protected override void InitializeActionMap()
    {
        RegisterAction(_inputActions.GenericUI.ScrollWheel, ctx => UserScroll(ctx));
        RegisterActionComplexCancel(_inputActions.GenericUI.Click, ctx => UserClick(ctx), ctx => UserClick(ctx));
    }

    private void UserScroll(InputAction.CallbackContext ctx)
    {
        // Zoom in or out based on the scroll input
        float scrollValue = ctx.ReadValue<Vector2>().y;

        if (scrollValue != 0)
        {
            _currentZoom += scrollValue * _zoomSpeed;
            _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);
            UpdateMapScale();
        }
    }

    private void UserClick(InputAction.CallbackContext ctx)
    {
        // Handle click events on the map
        if (ctx.phase == InputActionPhase.Performed)
        {
            Vector2 mousePosition = _inputActions.GenericUI.Point.ReadValue<Vector2>();
            _lastMousePosition = mousePosition;
            _isPanning = true;
        } 
        else if (ctx.phase == InputActionPhase.Canceled)
        {
            // Stop panning when the mouse button is released
            _isPanning = false;
        }
    }
    

    private void UpdateMapScale()
    {
        // Apply the zoom level to the map RectTransform
        _mapRectTransform.localScale = new Vector3(_currentZoom, _currentZoom, 1);

        // Clamp the map position to the maximum pan offset
        Vector2 newPosition = _mapRectTransform.anchoredPosition;
        newPosition.x = Mathf.Clamp(newPosition.x, -_maxPanOffset.x * _currentZoom, _maxPanOffset.x * _currentZoom);
        newPosition.y = Mathf.Clamp(newPosition.y, -_maxPanOffset.y * _currentZoom, _maxPanOffset.y * _currentZoom);
        _mapRectTransform.anchoredPosition = newPosition;
    }

    private void Update()
    {
        if (_isPanning)
        {
            // Get the current mouse position
            Vector2 currentMousePosition = _inputActions.GenericUI.Point.ReadValue<Vector2>();

            // Calculate the difference in mouse position
            Vector2 delta = currentMousePosition - _lastMousePosition;

            // Clamp the map position to the maximum pan offset
            Vector2 newPosition = _mapRectTransform.anchoredPosition + delta * _panSpeed;
            newPosition.x = Mathf.Clamp(newPosition.x, -_maxPanOffset.x * _currentZoom, _maxPanOffset.x * _currentZoom);
            newPosition.y = Mathf.Clamp(newPosition.y, -_maxPanOffset.y * _currentZoom, _maxPanOffset.y * _currentZoom);
            _mapRectTransform.anchoredPosition = newPosition;

            // Store the current mouse position for the next frame
            _lastMousePosition = currentMousePosition;
        }
    }

    private void Start() 
    {
        // Set the initial zoom level
        _currentZoom = _startZoom;

        // Create a map marker for each location
        for (int i = 0; i < _mapMarkerContainer.childCount; i++)
        {
            GameObject marker = Instantiate(_mapMarkerPrefab, _mapRectTransform);
            marker.GetComponentInChildren<TextMeshProUGUI>().text = _mapMarkerContainer.GetChild(i).name;
            Vector3 position = _mapMarkerContainer.GetChild(i).position;

            // Convert world position to map position
            Vector2 mapPosition = new Vector2(position.x / _worldSize.x * _mapSize.x, position.z / _worldSize.y * _mapSize.y);
            marker.GetComponent<RectTransform>().anchoredPosition = mapPosition;
        }
    }

}
