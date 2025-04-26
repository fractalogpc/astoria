
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEnableDisable : MonoBehaviour
{
	[SerializeField] private List<MonoBehaviour> _disableOnCutscene = new List<MonoBehaviour>();
	[SerializeField] private List<MonoBehaviour> _enableOnCutscene = new List<MonoBehaviour>();
	[SerializeField] private List<Camera> _disableOnCutsceneCamera = new List<Camera>();
	[SerializeField] private List<Camera> _enableOnCutsceneCamera = new List<Camera>();
	[SerializeField] private List<FadeElementInOut> _fadeOutOnCutscene = new List<FadeElementInOut>();
	
	public virtual void StartCutscene() {
		foreach (MonoBehaviour component in _disableOnCutscene) {
			if (component != null)
			component.enabled = false;
		}
		foreach (MonoBehaviour component in _enableOnCutscene) {
			if (component != null)
			component.enabled = true;
		}
		foreach (Camera cam in _disableOnCutsceneCamera) {
			if (cam != null)
			cam.enabled = false;
		}
		foreach (Camera cam in _enableOnCutsceneCamera) {
			if (cam != null)
				cam.enabled = true;
		}
		foreach (FadeElementInOut fade in _fadeOutOnCutscene) {
			print("Called fadeout");
			fade.FadeOut();
		}
	}
	public virtual void EndCutscene() {
		foreach (MonoBehaviour component in _disableOnCutscene) {
			component.enabled = true;
		}
		foreach (MonoBehaviour component in _enableOnCutscene) {
			component.enabled = false;
		}
		foreach (Camera cam in _disableOnCutsceneCamera) {
			cam.enabled = true;
		}
		foreach (Camera cam in _enableOnCutsceneCamera) {
				cam.enabled = false;
		}
		foreach (FadeElementInOut fade in _fadeOutOnCutscene) {
			fade.FadeIn();
		}
	}
}