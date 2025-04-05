
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEnableDisable : MonoBehaviour
{
	[SerializeField] private List<MonoBehaviour> _disableOnCutscene;
	[SerializeField] private List<MonoBehaviour> _enableOnCutscene;
	[SerializeField] private List<Camera> _disableOnCutsceneCamera;
	[SerializeField] private List<Camera> _enableOnCutsceneCamera;
	[SerializeField] private List<FadeElementInOut> _fadeOutOnCutscene;
	
	public virtual void StartCutscene() {
		foreach (MonoBehaviour component in _disableOnCutscene) {
			component.enabled = false;
		}
		foreach (MonoBehaviour component in _enableOnCutscene) {
			component.enabled = true;
		}
		foreach (Camera cam in _disableOnCutsceneCamera) {
			cam.enabled = false;
		}
		foreach (Camera cam in _enableOnCutsceneCamera) {
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