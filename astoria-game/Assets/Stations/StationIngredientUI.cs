using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StationIngredientUI : MonoBehaviour
{
	[SerializeField] private Image _itemIcon;
	[SerializeField] private TextMeshProUGUI _itemCount;

	public void SetItem(ItemData item, int count) {
		_itemIcon.sprite = item.ItemIcon;
		_itemIcon.preserveAspect = true;
		_itemCount.text = count.ToString();
	}
}