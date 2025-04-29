using UnityEngine;
using System.Collections;

public class CompassManager : MonoBehaviour
{
    
    [SerializeField] private Transform _pointer;
    [SerializeField] private Transform _target;
    [SerializeField] private float _spinSpeedWhenRandomized;

    private float _randomizeIntensity = 0;

    public void StartRandomizing()
    {
        StartCoroutine(SetRandomizeCoroutine(0, 1));
    }

    public void StopRandomizing()
    {
        StartCoroutine(SetRandomizeCoroutine(1, 0));
    }

    private IEnumerator SetRandomizeCoroutine(float start, float end)
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        _randomizeIntensity = start;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _randomizeIntensity = Mathf.Lerp(start, end, elapsedTime / duration);
            yield return null;
        }
        _randomizeIntensity = end;
    }

    private void Update()
    {
        if (_pointer != null && _target != null)
        {
            Vector3 direction = _target.position - _pointer.position;
            direction.y = 0;
            
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            float randomAngle = Mathf.PerlinNoise(Time.time * _spinSpeedWhenRandomized, 0) * 360f;
            angle += randomAngle * _randomizeIntensity;

            _pointer.rotation = Quaternion.Euler(0, angle, 0);
        }
    }

}
