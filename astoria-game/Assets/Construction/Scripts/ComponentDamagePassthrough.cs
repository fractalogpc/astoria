using Construction;
using UnityEngine;

public class ComponentDamagePassthrough : MonoBehaviour, IDamageable
{
    private ConstructionComponent component;

    private void Start() {
        component = GetComponentInParent<ConstructionComponent>();
        
        if (component == null)
        {
            Debug.LogError("ComponentDamagePassthrough: No ConstructionComponent found in parent!");
        }
    }

    public void TakeDamage(float damage, Vector3 hitPosition)
    {
        component.TakeDamage(damage, hitPosition);
    }
}
