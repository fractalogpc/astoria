using System;
using UnityEngine;

/// <summary>
/// Attached to a UI element that represents an inventory container.
/// </summary>
[AddComponentMenu("")]
public class InventoryContainerUI : MonoBehaviour
{
	public InventoryContainer AttachedContainer { get; private set; }
	[SerializeField] private CanvasGroup _greenHighlightGroup;
	[SerializeField] private CanvasGroup _redHighlightGroup;

	public void AttachContainer(InventoryContainer container) {
		DetachCurrentContainer();
		AttachedContainer = container;
		AttachedContainer.OnContainerUpdated += OnContainerUpdated;
	}
	private void DetachCurrentContainer() {
		if (AttachedContainer == null) return;
		AttachedContainer.OnContainerUpdated -= OnContainerUpdated;
		AttachedContainer = null;
	}

	private void OnContainerUpdated() {
		SetHighlightTo(AttachedContainer.Highlight);
	}
	private void SetHighlightTo(ContainerHighlight highlight) {
		switch (highlight) {
			case ContainerHighlight.None:
				_greenHighlightGroup.alpha = 0;
				_redHighlightGroup.alpha = 0;
				break;
			case ContainerHighlight.Green:
				_greenHighlightGroup.alpha = 1;
				_redHighlightGroup.alpha = 0;
				break;
			case ContainerHighlight.Red:
				_greenHighlightGroup.alpha = 0;
				_redHighlightGroup.alpha = 1;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(highlight), highlight, null);
		}
	}
	
	private void OnDestroy() {
		DetachCurrentContainer();
	}
}