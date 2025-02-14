using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class TimeCycleEvents : MonoBehaviour
{
    [ReadOnly][SerializeField] private TimeCycleCore _timeCycleCore;
    [Range(0, 24)][SerializeField] private int _periodStartHour;
    [Range(0, 24)][SerializeField] private int _periodEndHour;
    
    public UnityEvent PeriodStart;
    public UnityEvent PeriodEnd;

    private void OnValidate() {
        if (_periodStartHour > _periodEndHour) {
            _periodStartHour = _periodEndHour;
        }
    }
    private void Start() {
        _timeCycleCore = TimeCycleCore.Instance;
        _timeCycleCore.OnHourChanged.AddListener(OnHourChanged);
    }
    
    private void OnHourChanged() {
        if (_timeCycleCore.TimeOfDay.GameHour == _periodStartHour) {
            PeriodStart.Invoke();
        }
        if (_timeCycleCore.TimeOfDay.GameHour == _periodEndHour) {
            PeriodEnd.Invoke();
        }
    }
    
}
