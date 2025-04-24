using UnityEngine;

public class EnvironmentHolderManager : MonoBehaviour
{
    
    public static EnvironmentHolderManager InstanceIntroCutscene { get; private set; }
    public static EnvironmentHolderManager InstanceTransition { get; private set; }
    public static EnvironmentHolderManager InstanceGame { get; private set; }

    [SerializeField] private int environmentHolderIndex;

    private void Awake()
    {
        if (InstanceIntroCutscene == null && environmentHolderIndex == 0)
        {
            InstanceIntroCutscene = this;
        }
        else if (InstanceTransition == null && environmentHolderIndex == 1)
        {
            InstanceTransition = this;
        }
        else if (InstanceGame == null && environmentHolderIndex == 2)
        {
            InstanceGame = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
