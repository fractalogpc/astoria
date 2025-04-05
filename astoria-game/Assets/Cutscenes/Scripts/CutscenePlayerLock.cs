using System.Collections.Generic;
using UnityEngine;

public class CutscenePlayerLock : MonoBehaviour
{
	[SerializeField] private List<MonoBehaviour> _disableOnCutscene;
	[SerializeField] private List<MonoBehaviour> _enableOnCutscene;
	[SerializeField] private List<FadeElementInOut> _fadeOutOnCutscene;
	
	public void StartCutscene() {
		foreach (MonoBehaviour component in _disableOnCutscene) {
			component.enabled = false;
		}
		foreach (MonoBehaviour component in _enableOnCutscene) {
			component.enabled = true;
		}
		foreach (FadeElementInOut fade in _fadeOutOnCutscene) {
			fade.FadeOut();
		}
		InputReader.Instance.SwitchInputMap(InputMap.Cutscene);
	}
	public void EndCutscene() {
		foreach (MonoBehaviour component in _disableOnCutscene) {
			component.enabled = true;
		}
		foreach (MonoBehaviour component in _enableOnCutscene) {
			component.enabled = false;
		}
		foreach (FadeElementInOut fade in _fadeOutOnCutscene) {
			fade.FadeIn();
		}
		InputReader.Instance.SwitchInputMap(InputMap.Player);
	}
}