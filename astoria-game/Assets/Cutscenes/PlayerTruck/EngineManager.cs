using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EngineManager : Interactable
{
    public bool CanStart { get; private set; }
    [SerializeField] private ToggleUIVisibility _toggleUIVisibility;
    [SerializeField] private List<KeyItemSlot> _keyItemSlots;
    [SerializeField] private Image _engineImage;
    [SerializeField] private LocationObjectiveSource _sourceToCompleteOnRepair;
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
        if (!_sourceToCompleteOnRepair.Completed) {
            _sourceToCompleteOnRepair.ManualCompleteObjective();
        }
    }

    public override void Interact() {
        base.Interact();
        _toggleUIVisibility.ToggleVisibility();
    }
}
