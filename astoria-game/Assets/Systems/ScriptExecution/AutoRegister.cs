using System.Linq;
using UnityEngine;

/// <summary>
/// Automatically registers components with the ScriptExecutionManager
/// if they implement the specified execution interfaces.
/// </summary>
public class AutoRegister : MonoBehaviour
{
  private void Awake()
  {
    // Get all components on this GameObject that implement IOnEnableExecution
    var onEnableExecutionComponents = GetComponents<IOnEnableExecution>();
    foreach (var component in onEnableExecutionComponents.ToList())
    {
      // Register each component with the ScriptExecutionManager for OnEnable execution
      ScriptExecutionManager.Instance.RegisterScript(component);
    }

    // Get all components on this GameObject that implement IStartExecution
    var startExecutionComponents = GetComponents<IStartExecution>();
    foreach (var component in startExecutionComponents.ToList())
    {
      // Register each component with the ScriptExecutionManager for Start execution
      ScriptExecutionManager.Instance.RegisterScript(component);
    }
  }
}
