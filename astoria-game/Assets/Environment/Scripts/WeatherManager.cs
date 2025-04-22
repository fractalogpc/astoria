using System;
using System.Collections;
using UnityEngine;
public class WeatherManager : MonoBehaviour
{

    [Header("Weather Settings")]
    public WeatherType[] WeatherTypes;

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

    public void LerpToAtmosphere(string weatherType, float duration = 1f) {
        int index = GetWeatherType(weatherType, out WeatherType weather);
        if (index == -1) return;

        for (int i = 0; i < WeatherTypes.Length; i++)
        {
            if (i != index)
            {
                StartCoroutine(LerpValue(previousScreenWeights[i], 0f, duration, (value) => WeatherTypes[i].SetAtmosphereWeight(value)));
            }
        }
    }

    public void LerpToScreen(string weatherType, float duration = 1f) {
        int index = GetWeatherType(weatherType, out WeatherType weather);
        if (index == -1) return;

        for (int i = 0; i < WeatherTypes.Length; i++)
        {
            if (i != index)
            {
                StartCoroutine(LerpValue(previousScreenWeights[i], 0f, duration, (value) => WeatherTypes[i].SetScreenWeight(value)));
            }
        }
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
        [Range(0f, 1f)] public float AtmosphereWeight;
        [Range(0f, 1f)] public float ScreenWeight;
        [Range(0f, 1f)] public float EffectsWeight;
        public UnityEngine.Rendering.Volume AtmosphereVolume;
        public UnityEngine.Rendering.Volume ScreenVolume;
        public GameObject[] Effects;

        public void SetAtmosphereWeight(float weight)
        {
            AtmosphereWeight = weight;
            AtmosphereVolume.weight = weight;
        }

        public void SetScreenWeight(float weight)
        {
            ScreenWeight = weight;
            ScreenVolume.weight = weight;
        }

        public void SetEffectsWeight(float weight)
        {
            EffectsWeight = weight;
            if (Effects.Length > 0)
            {
                foreach (GameObject effect in Effects)
                {
                    effect.SetActive(weight > 0f);
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
