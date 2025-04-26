using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    
    [SerializeField] private GameObject[] objectsToEnableOnLoad;
    [SerializeField] private int framesPerObject = 1;

    private int _currentFrame = 0;
    private int _currentObjectIndex = 0;

    public void ForceLoadScene()
    {
        for (int i = 0; i < objectsToEnableOnLoad.Length; i++)
        {
            objectsToEnableOnLoad[i].SetActive(true);
        }
        _currentObjectIndex = objectsToEnableOnLoad.Length; // Set to the length to stop the update loop
    }

    private void Update()
    {
        if (_currentObjectIndex < objectsToEnableOnLoad.Length)
        {
            if (_currentFrame >= framesPerObject)
            {
                objectsToEnableOnLoad[_currentObjectIndex].SetActive(true);
                _currentObjectIndex++;
                _currentFrame = 0;
            }
            else
            {
                _currentFrame++;
            }
        }
    }

}
