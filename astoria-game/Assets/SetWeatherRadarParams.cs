using UnityEngine;
using System.Collections;

public class SetWeatherRadarParams : MonoBehaviour
{

    [SerializeField] private Vector2 _startWarpIntensity = new Vector2(0.1f, 0.1f);
    [SerializeField] private Vector2 _endWarpIntensity = new Vector2(0.5f, 0.5f);

    [SerializeField] private float _startTimeStep = 0.1f;
    [SerializeField] private float _endTimeStep = 0.5f;

    [SerializeField] private float _transitionTime = 1.0f;

    [SerializeField] private Renderer _weatherRadarRenderer;
    private MaterialPropertyBlock materialPropertyBlock;

    private void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
    }

    public void StartChange()
    {
        StartCoroutine(ChangeWeatherRadarParamsCoroutine(_startWarpIntensity, _endWarpIntensity, _startTimeStep, _endTimeStep));
    }

    private IEnumerator ChangeWeatherRadarParamsCoroutine(Vector2 startWarpIntensity, Vector2 endWarpIntensity, float startTimeStep, float endTimeStep)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _transitionTime);

            Vector2 currentWarpIntensity = Vector2.Lerp(startWarpIntensity, endWarpIntensity, t);
            float currentTimeStep = Mathf.Lerp(startTimeStep, endTimeStep, t);

            _weatherRadarRenderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetVector("_WarpIntensity", new Vector4(currentWarpIntensity.x, currentWarpIntensity.y, 0, 0));
            materialPropertyBlock.SetFloat("_TimeStepLength", currentTimeStep);
            _weatherRadarRenderer.SetPropertyBlock(materialPropertyBlock);

            yield return null;
        }
    }

}
