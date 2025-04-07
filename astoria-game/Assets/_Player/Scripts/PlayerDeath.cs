using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    [SerializeField] private Transform _respawnPoint; // The point where the player will respawn
    
    public void PlayerDie()
    {
        transform.position = _respawnPoint.position;
    }

}
