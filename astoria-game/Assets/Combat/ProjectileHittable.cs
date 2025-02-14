using UnityEngine;
using UnityEngine.Events;

public class ProjectileHittable : MonoBehaviour
{
    /// <summary>
    /// Called by CurrentLogic. Contains float damage.
    /// </summary>
    public UnityEvent<float, Vector3> OnProjectileHit;

    public void HitWithProjectile(float damage, Vector3 hitPosition) {
        Debug.Log($"{gameObject.name} hit for {damage} with projectile at {hitPosition}.", gameObject);
        OnProjectileHit?.Invoke(damage, hitPosition);
    }
}
