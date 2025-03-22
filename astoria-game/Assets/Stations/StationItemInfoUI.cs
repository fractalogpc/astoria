using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StationItemInfoUI : MonoBehaviour
{
	public bool Initialized = false;
	[SerializeField] [ReadOnly] private StationCore _station;
	[SerializeField] private TextMeshProUGUI _itemName;
	[SerializeField] private Image _itemImage;
	[SerializeField] private TextMeshProUGUI _descriptionText;
	[SerializeField] private Transform _ingredientList;
	[SerializeField] private GameObject _ingredientPrefab;

	public void Initialize(StationCore station) {
		if (Initialized) return;
		ResetInfo();
		_station = station;
		Initialized = true;
	}

	public void SetRecipe(RecipeData data) {
		if (data == null) {
			ResetInfo();
			return;
		}

		SetItemInfo(data._resultSetList.List[0].ItemData);
		RemoveAllChildrenOfList();
		foreach (ItemSet ingredient in data._ingredientSetList.List) {
			StationIngredientUI ingredientUI = Instantiate(_ingredientPrefab, _ingredientList).GetComponent<StationIngredientUI>();
			ingredientUI.SetItem(ingredient.ItemData, ingredient.ItemCount * _station.SelectedCraftCount);
		}
	}

	public void ResetInfo() {
		_itemName.text = "";
		_itemImage.sprite = null;
		_descriptionText.text = "";
		RemoveAllChildrenOfList();
	}

	private void SetItemInfo(ItemData item) {
		_itemName.text = item.ItemName;
		_itemImage.sprite = item.ItemIcon;
		_itemImage.preserveAspect = true;
		_descriptionText.text = item.ItemDescription;
	}

	private void RemoveAllChildrenOfList() {
		foreach (Transform child in _ingredientList) {
			Destroy(child.gameObject);
		}
	}
}