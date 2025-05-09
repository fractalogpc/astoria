using UnityEngine;
using UnityEngine.Events;

public class TogglePauseMenu : InputHandlerBase
{
    public UnityEvent OnPauseMenuOpen;
    public UnityEvent OnPauseMenuClose;

    InputMap stashedInputMap = InputMap.Player;

    public bool FreezeTimeOnPause = true;

    public bool IsPauseMenuOpen { get; private set; }
    [SerializeField] private CanvasGroup _pauseMenuCanvasGroup;

    protected override void InitializeActionMap()
    {
        RegisterAction(_inputActions.Player.Pause, ctx => SetVisibility(true));
        RegisterAction(_inputActions.GenericUI.CloseUI, ctx => SetVisibility(false));
    }

    public void SetVisibility(bool show)
    {
        IsPauseMenuOpen = show;
        _pauseMenuCanvasGroup.alpha = show ? 1 : 0;
        _pauseMenuCanvasGroup.blocksRaycasts = show;
        _pauseMenuCanvasGroup.interactable = show;
        if (show)
        {
            stashedInputMap = InputReader.Instance.CurrentMap;

            OnPauseMenuOpen?.Invoke();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            InputReader.Instance.SwitchInputMap(InputMap.GenericUI);

            if (FreezeTimeOnPause) {
                Time.timeScale = 0;
            }
        }
        else
        {
            OnPauseMenuClose?.Invoke();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            InputReader.Instance.SwitchInputMap(InputMap.Player);

            if (FreezeTimeOnPause) {
                Time.timeScale = 1.0f;
            }
        }
    }
}
