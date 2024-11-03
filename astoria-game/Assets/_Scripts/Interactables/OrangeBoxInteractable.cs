using UnityEngine;

public class OrangeBoxInteractable : Interactable
{
  [SerializeField] private Material _highlightMaterial;
  private Material _defaultMaterial;
  private Renderer _renderer;

  private bool _isHighlighted;

  private void Awake() {
    _renderer = GetComponent<Renderer>();
    _defaultMaterial = _renderer.material;
  }

  public override void Interact() {
    if (_isHighlighted) {
      _renderer.material = _defaultMaterial;
    } else {
      _renderer.material = _highlightMaterial;
    }

    _isHighlighted = !_isHighlighted;
  }
}
