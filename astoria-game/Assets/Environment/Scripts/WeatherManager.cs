using System;
using UnityEngine;
public class WeatherManager : MonoBehaviour
{

    [Header("Weather Settings")]
    public WeatherType[] WeatherTypes;

    private float[] previousWeights;

    private void Start()
    {
        previousWeights = new float[WeatherTypes.Length];
        for (int i = 0; i < WeatherTypes.Length; i++)
        {
            previousWeights[i] = -1f;
        }
    } 

    private void Update()
    {
        for (int i = 0; i < WeatherTypes.Length; i++)
        {
            if (WeatherTypes[i].Weight != previousWeights[i])
            {
                UpdateWeather(i);
                previousWeights[i] = WeatherTypes[i].Weight;
            }
        }
    }

    private void UpdateWeather(int index) {
        WeatherType weather = WeatherTypes[index];

        // Update volume settings
        if (weather.Volume != null)
        {
            weather.Volume.weight = weather.Weight;
        }

        // Activate or deactivate effects based on weight
        foreach (GameObject effect in weather.Effects)
        {
            if (effect != null)
            {
                effect.SetActive(weather.Weight > 0f);
            }
        }
    }

    [Serializable]
    public struct WeatherType {
        public string Name;
        [Range(0f, 1f)] public float Weight;
        public UnityEngine.Rendering.Volume Volume;
        public GameObject[] Effects;
    }
}
