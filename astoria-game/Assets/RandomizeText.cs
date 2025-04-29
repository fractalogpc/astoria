using UnityEngine;
using TMPro;

public class RandomizeText : MonoBehaviour
{
    
    [SerializeField] private int _characterCount = 10;
    [SerializeField] private float _randomizeSpeed = 0.1f;

    [SerializeField] private string _textToRandomize = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    private string _currentText = string.Empty;
    
    private float _timeSinceLastRandomization = 0f;

    private bool _isRandomizing = false;

    public void StartRandomizing()
    {
        _currentText = _textMeshProUGUI.text; // Store the current text
        _isRandomizing = true;
    }

    public void StopRandomizing()
    {
        _isRandomizing = false;
        _textMeshProUGUI.text = _currentText; // Reset to the current text when stopping
    }

    private void Update()
    {
        if (_isRandomizing)
        {
            _timeSinceLastRandomization += Time.deltaTime;
            
            if (_timeSinceLastRandomization >= _randomizeSpeed)
            {
                string randomText = string.Empty;
                for (int i = 0; i < _characterCount; i++)
                {
                    randomText += _textToRandomize[Random.Range(0, _textToRandomize.Length)];
                }
                
                _textMeshProUGUI.text = randomText;
                _timeSinceLastRandomization = 0f;
            }
        }
    }

}
