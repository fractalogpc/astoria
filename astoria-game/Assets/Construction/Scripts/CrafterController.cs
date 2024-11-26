using System.Collections.Generic;
using UnityEngine;

public class CrafterController : MonoBehaviour, IStartExecution
{
  private int _index = 0;
  private int Index {
    get { return _index; }
    set {
      if (value < 0) {
        value = datas.Length - 1;
      } else if (value >= datas.Length) {
        value = 0;
      }
      _index = value;
    }
  }

  [SerializeField] private ConstructableObjectData[] datas;

  [SerializeField] private TMPro.TextMeshProUGUI _text;
  [SerializeField] private TMPro.TextMeshProUGUI _errorText;

  private ConstructionCore _constructionCore;


  private bool _canCraft;

  public void IterateIndex(int index) {
    Index += index;

    _text.text = datas[Index].name;

    UpdateText();
  }

  public void InitializeStart() {
    if (datas.Length == 0) {
      Debug.LogWarning("No Constructable Object Data found.");
      Destroy(this);
      return;
    }
    _text.text = datas[Index].name;

    // LocalPlayerReference.Instance.Inventory().OnInventoryChange.AddListener(UpdateText);

    ResourceHolder.Instance.GameStateHandler.AddOnStateEnter(GameStateHandler.GameState.Playing, () => {
      Initialize();
    });
  }

  private void Initialize() {
    _constructionCore = LocalPlayerReference.Instance.LocalPlayer.GetComponent<ConstructionCore>();

    UpdateText();
  }

  private void OnDisable()
  {
    // idk, this is annoying. We should be unsubscribing from these events, however sometimes InventoryUI is destroyed before CrafterController resulting in a null error. If this makes an error ask me (Elliot) to fix it.
    // InventoryUI.Instance.OnInventoryChange.RemoveListener(UpdateText);
  }

  public void CreateObject() {
    if (!_canCraft) return;
    _constructionCore.TryGiveObject(datas[Index]);

    foreach (var item in datas[Index].Cost) {
      LocalPlayerReference.Instance.Inventory().TryRemoveItemByData(item.Item, item.Amount);
    }
  }

  private void UpdateText(List<InventoryItem> items = null)
  {
    string errorText = "";
    _canCraft = _constructionCore.CanGiveObject(datas[Index], out errorText);
    if (_canCraft) {
      _errorText.text = "";
    } else {
      _errorText.text = errorText;
    }
  }
}
