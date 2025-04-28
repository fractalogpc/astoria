using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DisableRig : MonoBehaviour
{
    
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private Rig rig;

    public void DisableRigBuilder()
    {
        rig.weight = 0f;
        rigBuilder.Build();
    }
}
