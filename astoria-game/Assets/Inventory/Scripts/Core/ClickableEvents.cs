using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickableEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent OnHoverOn;
    public UnityEvent OnHoverOff;
    public UnityEvent OnClickDownAnywhere;
    public UnityEvent OnClickUpAnywhere;
    public UnityEvent OnClickDownSelected;
    public UnityEvent OnClickUpSelected;
    
    public bool _isHovering;

    public void OnPointerEnter(PointerEventData eventData) {
        _isHovering = true;
        OnHoverOn?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData) {
        _isHovering = false;
        OnHoverOff?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData) {
        OnClickDownAnywhere.Invoke();
        if (!_isHovering) return;
        OnClickDownSelected?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData) {
        OnClickUpAnywhere?.Invoke();
        // Debug.Log("Pointer up", gameObject);
        if (!_isHovering) return;
        OnClickUpSelected?.Invoke();
    }
}
