using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Events;

public class WeatherManager : MonoBehaviour
{

    [System.Serializable]
    private struct WeatherState
    {

        public string name;
        public Volume volume;
        public float weight;
        public UnityEvent onEnter;
        public UnityEvent onExit;

    }

    [SerializeField] private WeatherState[] _weatherStates;

    [SerializeField] private float _transitionTime = 1.0f;
    [SerializeField] private float _transitionTimeVariance = 0.5f;
    [SerializeField] private int _framesPerUpdate = 1;
    [SerializeField] private float _transitionEventThreshold = 0.5f;
    [SerializeField] private float _averageTimeBetweenWeatherChanges = 10.0f;
    [SerializeField] private float _timeBetweenWeatherChangesVariance = 5.0f;

    private WeatherState _currentWeatherState;
    private WeatherState _targetWeatherState;
    private float _timeToNextWeatherChange;

    private void Start()
    {
        _timeToNextWeatherChange = _averageTimeBetweenWeatherChanges + Random.Range(-_timeBetweenWeatherChangesVariance, _timeBetweenWeatherChangesVariance);
        _currentWeatherState = GetRandomWeatherState();
        _currentWeatherState.volume.weight = 1.0f;
    }

    private WeatherState GetRandomWeatherState()
    {
        float totalWeight = 0.0f;
        foreach (WeatherState weatherState in _weatherStates)
        {
            totalWeight += weatherState.weight;
        }

        float randomValue = Random.Range(0.0f, totalWeight);
        foreach (WeatherState weatherState in _weatherStates)
        {
            randomValue -= weatherState.weight;
            if (randomValue <= 0.0f)
            {
                return weatherState;
            }
        }
        return _weatherStates[0];
    }

    private void Update()
    {
        _timeToNextWeatherChange -= Time.deltaTime;
        if (_timeToNextWeatherChange <= 0.0f)
        {
            _targetWeatherState = GetRandomWeatherState();
            _timeToNextWeatherChange = _averageTimeBetweenWeatherChanges + Random.Range(-_timeBetweenWeatherChangesVariance, _timeBetweenWeatherChangesVariance);
            StartCoroutine(TransitionWeather());
        }
    }

    private IEnumerator TransitionWeather()
    {
        float elapsedTime = 0.0f;
        float transitionTime = _transitionTime + Random.Range(-_transitionTimeVariance, _transitionTimeVariance);
        int frameCount = 0;
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            frameCount++;
            float t = elapsedTime / transitionTime;
            if (frameCount >= _framesPerUpdate)
            {
                frameCount = 0;
                _currentWeatherState.volume.weight = 1.0f - t;
                _targetWeatherState.volume.weight = t;
            }
            if (t >= _transitionEventThreshold) {
                _currentWeatherState.onExit.Invoke();
                _targetWeatherState.onEnter.Invoke();
            }
            yield return null;
        }
        _currentWeatherState.volume.weight = 0.0f;
        _targetWeatherState.volume.weight = 1.0f;
        _currentWeatherState = _targetWeatherState;
    }

}
