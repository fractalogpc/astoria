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

  [SerializeField] private TMPro.TextMeshProUGUI text;

  public ConstructionCore ConstructionCore;

  public void IterateIndex(int index) {
    Index += index;

    text.text = datas[Index].name;
  }

  public void InitializeStart() {
    if (datas.Length == 0) {
      Debug.LogWarning("No Constructable Object Data found.");
      Destroy(this);
      return;
    }
    text.text = datas[Index].name;
  }

  public void CreateObject() {
    ConstructionCore.TryGiveObject(datas[Index]);
  }
}
