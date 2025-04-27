using UnityEngine;
using TMPro;

public class AnimatedText : MonoBehaviour
{
    
    [SerializeField] private string textToScroll = "example scrolling text";

    // Maybe expand this later but right now it's purpose-built for the radio in the car.
    // Scrolls text from right to left across a little text box, assuming truncation on the left side.
    // Also repeats the text infinitely.

    [SerializeField] private float timePerCharacter = 0.2f;

    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    private string currentText = string.Empty;
    private int currentIndex = 0;
    private float timeSinceLastCharacter = 0f;

    private void Update()
    {
        if (_textMeshProUGUI == null) return; // Ensure the TextMeshProUGUI component is assigned
        timeSinceLastCharacter += Time.deltaTime;
        
        if (timeSinceLastCharacter >= timePerCharacter)
        {
            currentText += textToScroll[currentIndex];
            _textMeshProUGUI.text = currentText;
            currentIndex++;
            timeSinceLastCharacter = 0f;

            currentIndex %= textToScroll.Length; // Loop back to the start of the text
        }
    }

}
