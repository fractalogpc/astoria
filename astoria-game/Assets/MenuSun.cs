using FMODUnity;
using UnityEngine;

public class MenuSun : MonoBehaviour
{

    private int lanternsOn = 0;

    public GameObject rain;
    public GameObject lightning;
    public Quaternion rotation;
    private Quaternion stashedRotation;

    void Start()
    {
        rain.SetActive(false);
        stashedRotation = transform.rotation;
    }

    public void LanternClick(bool enabled)
    {
        Debug.Log("Lantern clicked: " + enabled);
        if (enabled)
        {
            lanternsOn++;
            if (lanternsOn == 3)
            {
                Fun();
            }
        }
        else
        {
            if (lanternsOn == 3)
            {
                Undo();
            }
            lanternsOn--;
        }
    }

    private void Fun()
    {
        rain.SetActive(true);
        lightning.SetActive(true);
        transform.rotation = rotation;
    }

    private void Undo()
    {
        rain.SetActive(false);
        lightning.SetActive(false);
        transform.rotation = stashedRotation;
    }
}
