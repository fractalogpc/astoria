using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingIngredientUI : MonoBehaviour
{
	[SerializeField] private Image _itemIcon;
	[SerializeField] private TextMeshProUGUI _itemCount;

	public void SetItem(ItemData item, int count) {
		_itemIcon.sprite = item.ItemIcon;
		_itemCount.text = count.ToString();
	}
}