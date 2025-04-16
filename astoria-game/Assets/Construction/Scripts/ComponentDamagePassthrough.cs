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
        Debug.Log("im takin damage bro help me vro pls");
        component.TakeDamage(damage, hitPosition);
    }
}
