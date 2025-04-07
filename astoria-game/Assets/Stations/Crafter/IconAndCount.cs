using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconAndCount : MonoBehaviour
{
	[SerializeField] private Image _itemIcon;
	[SerializeField] private TextMeshProUGUI _itemCount;

	public void SetIconAndCount(ItemData item, int count) {
		_itemIcon.sprite = item.ItemIcon;
		_itemIcon.preserveAspect = true;
		_itemCount.text = count.ToString();
	}
}