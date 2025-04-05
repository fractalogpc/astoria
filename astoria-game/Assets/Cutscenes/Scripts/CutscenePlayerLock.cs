using System.Collections.Generic;
using UnityEngine;

public class CutscenePlayerLock : CutsceneEnableDisable
{
	public override void StartCutscene() {
		base.StartCutscene();
		InputReader.Instance.SwitchInputMap(InputMap.Cutscene);
	}
	public override void EndCutscene() {
		base.EndCutscene();
		InputReader.Instance.SwitchInputMap(InputMap.Player);
	}
}