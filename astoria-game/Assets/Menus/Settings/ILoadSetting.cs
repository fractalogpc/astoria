using UnityEngine;

public interface ILoadSetting
{

    public void StartMethod()
    {
        ReloadSetting();

        SettingsManager.Instance.OnSettingsChanged.AddListener(ReloadSetting);
    }

    public abstract void ReloadSetting();

}