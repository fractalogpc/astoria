using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingItemInfoUI : MonoBehaviour
{
	public bool Initialized = false;
	[SerializeField] [ReadOnly] private CraftingStationNetworked _craftingStation;
	[SerializeField] private TextMeshProUGUI _itemName;
	[SerializeField] private Image _itemImage;
	[SerializeField] private TextMeshProUGUI _descriptionText;
	[SerializeField] private Transform _ingredientList;
	[SerializeField] private GameObject _ingredientPrefab;

	public void Initialize(CraftingStationNetworked craftingStation) {
		if (Initialized) return;
		ResetInfo();
		_craftingStation = craftingStation;
		Initialized = true;
	}

	public void SetRecipe(RecipeData data) {
		if (data == null) {
			ResetInfo();
			return;
		}

		SetItemInfo(data._resultSetList.ItemSets[0].ItemData);
		RemoveAllChildrenOfList();
		foreach (ItemSet ingredient in data._ingredientSetList.ItemSets) {
			CraftingIngredientUI ingredientUI = Instantiate(_ingredientPrefab, _ingredientList).GetComponent<CraftingIngredientUI>();
			ingredientUI.SetItem(ingredient.ItemData, ingredient.ItemCount * _craftingStation.SelectedCraftCount);
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
		_descriptionText.text = item.ItemDescription;
	}

	private void RemoveAllChildrenOfList() {
		foreach (Transform child in _ingredientList) {
			Destroy(child.gameObject);
		}
	}
}