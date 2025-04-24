using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSelectionManager : MonoBehaviour
{
    
    [SerializeField] private Image thumbnailImage;
    [SerializeField] private TextMeshProUGUI saveNameText;
    [SerializeField] private Sprite defaultThumbnail;

    [SerializeField] private SaveSystem saveSystem;

    private SaveSystem.SaveInfo[] saveInfos;
    private int currentIndex = 0;

    private void Start()
    {
        if (saveSystem == null)
        {
            Debug.LogError("SaveSystem reference is not set.");
            return;
        }

        RebuildMenu();
    }

    public void MoveSelection(int direction)
    {
        if (saveInfos.Length == 0)
        {
            Debug.LogError("No saves available to select.");
            return;
        }

        currentIndex += direction;

        if (currentIndex < 0)
        {
            currentIndex = saveInfos.Length - 1;
        }
        else if (currentIndex >= saveInfos.Length)
        {
            currentIndex = 0;
        }

        DisplayCurrentSave();
    }

    public void Delete()
    {
        if (saveInfos.Length == 0)
        {
            Debug.LogError("No saves available to delete.");
            return;
        }

        saveSystem.DeleteSave(currentIndex);
        RebuildMenu();
    }

    public void Load()
    {
        if (saveInfos.Length == 0)
        {
            Debug.LogError("No saves available to load.");
            return;
        }

        saveSystem.LoadSave(currentIndex);
    }

    public void RebuildMenu()
    {
        saveInfos = saveSystem.GetSaveInfos();
        currentIndex = 0;

        if (saveInfos.Length > 0)
        {
            DisplayCurrentSave();
        } else {
            saveNameText.text = "No saves available.";
            thumbnailImage.sprite = defaultThumbnail;
        }
    }

    private void DisplayCurrentSave()
    {
        if (currentIndex < 0 || currentIndex >= saveInfos.Length)
        {
            Debug.LogError("Current index is out of bounds.");
            return;
        }

        SaveSystem.SaveInfo currentSaveInfo = saveInfos[currentIndex];
        saveNameText.text = currentSaveInfo.saveName;

        // Load thumbnail image here if available
        // thumbnailImage.sprite = LoadThumbnail(currentSaveInfo);
        thumbnailImage.sprite = defaultThumbnail;
    }

}
