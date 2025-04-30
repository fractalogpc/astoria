using UnityEngine;

public interface ILoadSetting
{

    private void Start()
    {
        ReloadSetting();

        SettingsManager.Instance.OnSettingsChanged.AddListener(ReloadSetting);
    }

    public abstract void ReloadSetting();

}