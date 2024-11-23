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


  private bool _canCraft;

  public void IterateIndex(int index) {
    Index += index;

    _text.text = datas[Index].name;
  }

  public void InitializeStart() {
    if (datas.Length == 0) {
      Debug.LogWarning("No Constructable Object Data found.");
      Destroy(this);
      return;
    }
    _text.text = datas[Index].name;
  }

  public void CreateObject() {
    if (!_canCraft) return;
    ResourceHolder.Instance.ConstructionCore.TryGiveObject(datas[Index]);

    foreach (var item in datas[Index].Cost) {
      InventoryUI.Instance.TryRemoveItemByData(item.Item, item.Amount);
    }
  }

  private void Update()
  {
    string errorText = "";
    _canCraft = ResourceHolder.Instance.ConstructionCore.CanGiveObject(datas[Index], out errorText);
    if (_canCraft) {
      _errorText.text = "";
    } else {
      _errorText.text = errorText;
    }
  }
}
