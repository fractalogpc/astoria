using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Random = UnityEngine.Random;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Create Waypoints", story: "[Agent] creates [waypoints] from terrain", category: "Action", id: "d2691bc7dfdd9b25856ba8d5e1dde67b")]
public partial class CreateWaypointsAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Waypoints;

    protected override Status OnStart()
    {
        List<GameObject> points = new List<GameObject>();
        for (int i = 0; i < 5; i++) {
            float x = Random.value * 100 + Agent.Value.transform.position.x;
            float z =  Random.value * 100 + Agent.Value.transform.position.z;
            float y = 0;
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(x, 10000, z), Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"))) {
                y = 10000 - hit.distance; 
            }

            points.Add(UnityEngine.Object.Instantiate(new GameObject(), new Vector3(x, y, z), Quaternion.identity));
        }
        Waypoints.Value = points;
        Debug.Log(Waypoints.Value[0].ToString());
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

