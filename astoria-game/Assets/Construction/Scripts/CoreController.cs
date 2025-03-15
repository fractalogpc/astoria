using UnityEngine;
using Mirror;
using Construction;
using UnityEngine.Events;

/// <summary>
/// Handles the CORE prop. Not to be confused with ConstructionCore.
/// </summary>
public class CoreController : MonoBehaviour, IDamageable
{

    [SerializeField] private float maxHealth = 100f;

    public UnityEvent OnCoreDestroyed;

    private float health = 100f;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage, Vector3 hitPoint)
    {
        health -= damage;
        if (health <= 0)
        {
            CoreDestroyed();
        }
    }

    private void CoreDestroyed()
    {
        OnCoreDestroyed.Invoke();
    }

}
