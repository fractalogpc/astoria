using UnityEngine;

public class PlayerSpawnpoint : MonoBehaviour
{
    public int Priority => _priority;
    
    [Tooltip("The lower the index, the higher the priority.")]
    [SerializeField] private int _priority = 0;
}
