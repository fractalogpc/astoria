using Construction;
using UnityEngine;

public class ComponentDamagePassthrough : MonoBehaviour, IDamageable
{
    private ConstructionComponent component;
    private HealthManager healthManager;

    private void Start() {
        component = GetComponentInParent<ConstructionComponent>();
        healthManager = GetComponentInParent<HealthManager>();
        
        if (component == null)
        {
            Debug.LogError("ComponentDamagePassthrough: No ConstructionComponent found in parent!");
        }
    }

    public void TakeDamage(float damage, Vector3 hitPosition)
    {
        Debug.Log("im takin damage bro help me vro pls");
        if (component != null) component.TakeDamage(damage, hitPosition);
        if (healthManager != null) healthManager.TakeDamage(damage, hitPosition);
    }
}
