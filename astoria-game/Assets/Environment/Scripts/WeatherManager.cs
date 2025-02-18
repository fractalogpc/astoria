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
        public float snowIntensity;
        public float wetnessIntensity;
        public float drynessIntensity;
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
    [SerializeField] private TheVisualEngine.TVEManager _tveManager;

    private WeatherState _currentWeatherState;
    private WeatherState _targetWeatherState;
    private float _timeToNextWeatherChange;

    private void Start()
    {
        _timeToNextWeatherChange = _averageTimeBetweenWeatherChanges + Random.Range(-_timeBetweenWeatherChangesVariance, _timeBetweenWeatherChangesVariance);
        _currentWeatherState = GetRandomWeatherState();
        _currentWeatherState.volume.weight = 1.0f;
        _currentWeatherState.onEnter.Invoke();
        _tveManager.globalAtmoData.overlayIntensity = _currentWeatherState.snowIntensity;
        _tveManager.globalAtmoData.wetnessIntensity = _currentWeatherState.wetnessIntensity;
        _tveManager.globalAtmoData.drynessIntensity = _currentWeatherState.drynessIntensity;
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
        if (_currentWeatherState.name == _targetWeatherState.name)
        {
            yield break;
        }
        float elapsedTime = 0.0f;
        float transitionTime = _transitionTime + Random.Range(-_transitionTimeVariance, _transitionTimeVariance);
        int frameCount = _framesPerUpdate;
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
                _tveManager.globalAtmoData.overlayIntensity = Mathf.Lerp(_currentWeatherState.snowIntensity, _targetWeatherState.snowIntensity, t);
                _tveManager.globalAtmoData.wetnessIntensity = Mathf.Lerp(_currentWeatherState.wetnessIntensity, _targetWeatherState.wetnessIntensity, t);
                _tveManager.globalAtmoData.drynessIntensity = Mathf.Lerp(_currentWeatherState.drynessIntensity, _targetWeatherState.drynessIntensity, t);
            }
            if (t >= _transitionEventThreshold) {
                _currentWeatherState.onExit.Invoke();
                _targetWeatherState.onEnter.Invoke();
            }
            yield return null;
        }
        _currentWeatherState.volume.weight = 0.0f;
        _targetWeatherState.volume.weight = 1.0f;
        _tveManager.globalAtmoData.overlayIntensity = _targetWeatherState.snowIntensity;
        _tveManager.globalAtmoData.wetnessIntensity = _targetWeatherState.wetnessIntensity;
        _tveManager.globalAtmoData.drynessIntensity = _targetWeatherState.drynessIntensity;
        _currentWeatherState = _targetWeatherState;
    }

}
