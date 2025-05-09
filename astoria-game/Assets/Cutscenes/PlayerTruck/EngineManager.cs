using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EngineManager : MonoBehaviour
{
    public bool CanStart { get; private set; } 
    [SerializeField] private List<KeyItemSlot> _keyItemSlots;
    [SerializeField] private Image _engineImage;
    [SerializeField][ColorUsage(true, false)] private Color _notWorkingColor;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _engineImage.color = _notWorkingColor;
    }

    // Update is called once per frame
    void Update() {
        CanStart = _keyItemSlots.All(keyItem => keyItem.RequirementMet);
        if (!CanStart) {
            _engineImage.color = _notWorkingColor;
            return;
        }
        _engineImage.color = Color.white;
    }
}
