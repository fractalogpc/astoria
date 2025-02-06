using UnityEngine;

public class HarvesterData : BaseToolData
{
    [Tooltip("The amount each hit adds to the harvestable's damage. When the damage exceeds the limit on the harvestable, the minigame will start.")]
    public float HarvestDamage;
    [Tooltip("The size of the additional items zone as a percentage of the full bar.")]
    [Range(0.1f, 100f)] public float CritWidthPercent;
}
