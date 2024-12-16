using UnityEngine;
using UnityEngine.Events;

public class ProjectileHittable : MonoBehaviour
{
    /// <summary>
    /// Called by CurrentLogic. Contains float damage.
    /// </summary>
    public UnityEvent<float> OnProjectileHit;

    public void HitWithProjectile(float damage) {
        Debug.Log($"{gameObject.name} hit with projectile.");
        OnProjectileHit?.Invoke(damage);
    }
}
