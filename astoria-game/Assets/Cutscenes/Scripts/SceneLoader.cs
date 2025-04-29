using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour
{

    [System.Serializable]
    private struct LoadStep
    {
        public GameObject[] objectsToEnable;
        public MonoBehaviour[] componentsToEnable;
        public int framesPerObject;
    }
    
    [SerializeField] private LoadStep[] loadSteps;
    [SerializeField] private int framesPerStep = 1;

    private int _currentFrame = 0;
    private int _currentObjectIndex = 0;

    public void ForceLoadScene()
    {
        foreach (var step in loadSteps)
        {
            foreach (var obj in step.objectsToEnable)
            {
                obj.SetActive(true);
            }
        }
        _currentObjectIndex = loadSteps.Length; // Skip to the end
    }

    private void Awake()
    {
        // Start the loading process
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        foreach (var step in loadSteps)
        {
            foreach (var obj in step.objectsToEnable)
            {
                obj.SetActive(true);
                yield return new WaitForSeconds(step.framesPerObject * Time.deltaTime);
            }
            foreach (var component in step.componentsToEnable)
            {
                component.enabled = true;
            }
            yield return new WaitForSeconds(framesPerStep * Time.deltaTime);
        }
    }

}
