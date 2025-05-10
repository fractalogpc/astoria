using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigToggle : MonoBehaviour
{
    
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private Rig rig;

    public void DisableRigBuilder()
    {
        rig.weight = 0f;
        rigBuilder.Build();
    }
    
    public void EnableRigBuilder()
    {
        rig.weight = 1f;
        rigBuilder.Build();
    }
}
