using UnityEngine;
using TMPro;

public class PickRandomText : MonoBehaviour
{
    
    [SerializeField] private string[] _texts;

    private void Awake()
    {
        GetComponent<TextMeshProUGUI>().text = _texts[Random.Range(0, _texts.Length)];
    }

}
