using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
public class WeatherManager : MonoBehaviour
{

    [Header("Weather Settings")]
    [SerializeField] private bool _randomlyChangeWeather = false;
    [SerializeField] private float _weatherChangeInterval = 5f;
    [SerializeField] private float _weatherChangeDuration = 1f;

    public WeatherType[] WeatherTypes;
    // Sunny, Overcast, Rainy, Stormy, Snowy, Ash, Dry, Windy

    private float[] previousAtmosphereWeights;
    private float[] previousScreenWeights;
    private float[] previousEffectsWeights;

    private void Start()
    {
        previousAtmosphereWeights = new float[WeatherTypes.Length];
        for (int i = 0; i < WeatherTypes.Length; i++)
        {
            previousAtmosphereWeights[i] = -1f;
        }

        previousScreenWeights = new float[WeatherTypes.Length];
        for (int i = 0; i < WeatherTypes.Length; i++)
        {
            previousScreenWeights[i] = -1f;
        }

        previousEffectsWeights = new float[WeatherTypes.Length];
        for (int i = 0; i < WeatherTypes.Length; i++)
        {
            previousEffectsWeights[i] = -1f;
        }
    }

    private void Update()
    {
        if (_randomlyChangeWeather)
        {
            // Randomly change weather every _weatherChangeInterval seconds
            if (Time.time % _weatherChangeInterval < Time.deltaTime && Time.time >= _weatherChangeInterval)
            {
                // Pick random weather effect based on weights
                int totalWeight = 0;
                for (int i = 0; i < WeatherTypes.Length; i++)
                {
                    totalWeight += WeatherTypes[i].WeightInRandomChoice;
                }

                int randomValue = UnityEngine.Random.Range(0, totalWeight);
                int randomIndex = 0;
                for (int i = 0; i < WeatherTypes.Length; i++)
                {
                    randomValue -= WeatherTypes[i].WeightInRandomChoice;
                    if (randomValue < 0)
                    {
                        randomIndex = i;
                        break;
                    }
                }

                LerpToAtmosphere(WeatherTypes[randomIndex].Name, 1f, _weatherChangeDuration);
                LerpToScreen(WeatherTypes[randomIndex].Name, _weatherChangeDuration);
                LerpToEffects(WeatherTypes[randomIndex].Name, _weatherChangeDuration);
            }
        }
    }

    public void LerpToAtmosphere(string weatherType, float value = 1f, float duration = 1f, bool clear = true)
    {
        int index = GetWeatherType(weatherType, out WeatherType weather);
        if (index == -1) return;

        if (clear)
        {
            // Clear previous weights
            for (int i = 0; i < WeatherTypes.Length; i++)
            {
                if (i != index)
                {
                    StartCoroutine(LerpValue(previousScreenWeights[i], 0f, duration, (value) => WeatherTypes[i].AtmosphereWeight = value));
                }
            }
        }

        // Set the current weather type
        StartCoroutine(LerpValue(previousAtmosphereWeights[index], value, duration, (value) => WeatherTypes[index].AtmosphereWeight = value));
    }

    public void LerpToScreen(string weatherType, float duration = 1f)
    {
        int index = GetWeatherType(weatherType, out WeatherType weather);
        if (index == -1) return;

        for (int i = 0; i < WeatherTypes.Length; i++)
        {
            if (i != index)
            {
                StartCoroutine(LerpValue(previousScreenWeights[i], 0f, duration, (value) => WeatherTypes[i].ScreenWeight = value));
            }
        }

        // Set the current weather type
        StartCoroutine(LerpValue(previousScreenWeights[index], 1f, duration, (value) => WeatherTypes[index].ScreenWeight = value));
    }

    public void LerpToEffects(string weatherType, float duration = 1f)
    {
        int index = GetWeatherType(weatherType, out WeatherType weather);
        if (index == -1) return;

        for (int i = 0; i < WeatherTypes.Length; i++)
        {
            if (i != index)
            {
                StartCoroutine(LerpValue(previousEffectsWeights[i], 0f, duration, (value) => WeatherTypes[i].EffectsWeight = value));
            }
        }

        // Set the current weather type
        StartCoroutine(LerpValue(previousEffectsWeights[index], 1f, duration, (value) => WeatherTypes[index].EffectsWeight = value));
    }

    public void SetAtmosphere(string weatherType, float value)
    {
        int index = GetWeatherType(weatherType, out WeatherType weather);
        if (index == -1) return;

        // Set the current weather type
        WeatherTypes[index].AtmosphereWeight = value;
    }

    public void SetScreen(string weatherType, float value)
    {
        int index = GetWeatherType(weatherType, out WeatherType weather);
        if (index == -1) return;

        // Set the current weather type
        WeatherTypes[index].ScreenWeight = value;
    }

    public void SetEffects(string weatherType, float value)
    {
        int index = GetWeatherType(weatherType, out WeatherType weather);
        if (index == -1) return;

        // Set the current weather type
        WeatherTypes[index].EffectsWeight = value;
    }

    public void SetWeight(string weatherType, float value)
    {
        int index = GetWeatherType(weatherType, out WeatherType weather);
        if (index == -1) return;

        // Set the current weather type
        WeatherTypes[index].AtmosphereWeight = value;
        WeatherTypes[index].ScreenWeight = value;
        WeatherTypes[index].EffectsWeight = value;
    }

    IEnumerator LerpValue(float from, float to, float duration, System.Action<float> onUpdate)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float value = Mathf.Lerp(from, to, t);
            onUpdate(value);
            yield return null;
        }

        // Ensure final value is set
        onUpdate(to);
    }

    private void ClearAtmosphere()
    {
        for (int i = 0; i < WeatherTypes.Length; i++)
        {
            WeatherTypes[i].SetAtmosphereWeight(0f);
        }
    }

    private void ClearScreen()
    {
        for (int i = 0; i < WeatherTypes.Length; i++)
        {
            WeatherTypes[i].SetScreenWeight(0f);
        }
    }

    private void ClearEffects()
    {
        for (int i = 0; i < WeatherTypes.Length; i++)
        {
            WeatherTypes[i].SetEffectsWeight(0f);
        }
    }

    private int GetWeatherType(string name, out WeatherType weatherType)
    {
        for (int i = 0; i < WeatherTypes.Length; i++)
        {
            if (WeatherTypes[i].Name == name)
            {
                weatherType = WeatherTypes[i];
                return i;
            }
        }

        Debug.LogError("Weather type not found: " + name);
        weatherType = default(WeatherType);
        return -1;
    }

    [Serializable]
    public struct WeatherType
    {
        public string Name;
        public int WeightInRandomChoice;
        public float AtmosphereWeight { get => atmosphereWeight; set => SetAtmosphereWeight(value); }
        public float ScreenWeight { get => screenWeight; set => SetScreenWeight(value); }
        public float EffectsWeight { get => effectsWeight; set => SetEffectsWeight(value); }

#if UNITY_EDITOR
        [UnityEditor.CustomEditor(typeof(WeatherManager))]
        public class WeatherManagerEditor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                WeatherManager manager = (WeatherManager)target;

                if (GUILayout.Button("Update Weights"))
                {
                    foreach (var weatherType in manager.WeatherTypes)
                    {
                        weatherType.SetAtmosphereWeight(weatherType.AtmosphereWeight);
                        weatherType.SetScreenWeight(weatherType.ScreenWeight);
                        weatherType.SetEffectsWeight(weatherType.EffectsWeight);
                    }
                }
            }
        }
#endif

        [SerializeField, Range(0f, 1f)] private float atmosphereWeight;
        [SerializeField, Range(0f, 1f)] private float screenWeight;
        [SerializeField, Range(0f, 1f)] private float effectsWeight;
        public UnityEngine.Rendering.Volume AtmosphereVolume;
        public UnityEngine.Rendering.Volume ScreenVolume;
        public GameObject[] Effects;

        public void SetAtmosphereWeight(float weight)
        {
            atmosphereWeight = weight;
            if (AtmosphereVolume != null)
                AtmosphereVolume.weight = weight;
        }

        public void SetScreenWeight(float weight)
        {
            screenWeight = weight;
            if (ScreenVolume != null)
                ScreenVolume.weight = weight;
        }

        public void SetEffectsWeight(float weight)
        {
            effectsWeight = weight;
            if (Effects.Length > 0)
            {
                foreach (GameObject effect in Effects)
                {
                    effect.SetActive(weight > 0f);
                    if (effect.TryGetComponent<VisualEffect>(out VisualEffect ve))
                    {
                        ve.SetFloat("Intensity", Mathf.Pow(weight, 2f));
                    }
                }
            } else {
                foreach(GameObject effect in Effects)
                {
                    effect.SetActive(false);
                    if (effect.TryGetComponent<VisualEffect>(out VisualEffect ve))
                    {
                        ve.SetFloat("Intensity", 0f);
                    }
                }
            }
        }

        public void Clear()
        {
            SetAtmosphereWeight(0f);
            SetScreenWeight(0f);
            SetEffectsWeight(0f);
        }
    }
}
