using UnityEngine;
using UnityEngine.Events;

public class ProjectileHittable : MonoBehaviour
{
    /// <summary>
    /// Called by GunLogic.
    /// </summary>
    public UnityEvent OnProjectileHit;

    public void HitWithProjectile() {
        Debug.Log($"{gameObject.name} hit with projectile.");
        OnProjectileHit?.Invoke();
    }
}
