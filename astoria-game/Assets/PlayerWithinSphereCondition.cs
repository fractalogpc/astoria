using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Player within Sphere", story: "[Agent] is in proximity to [Player] within radius [Radius]", category: "Conditions", id: "1ae8c82fac0af5f1c6ef5701ca9e8568")]
public partial class PlayerWithinSphereCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> Radius;

    public override bool IsTrue()
    {
        Collider[] hitColliders = Physics.OverlapSphere(Agent.Value.transform.position, Radius.Value, 12);
        foreach (var hitCollider in hitColliders) {
            if (hitCollider.tag == "Player") {
                Player.Value = hitCollider.gameObject;
                return true;
            } else {
                return false;
            }
        }
        return false;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
