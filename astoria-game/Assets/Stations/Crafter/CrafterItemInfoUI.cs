using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrafterItemInfoUI : MonoBehaviour
{
	public bool Initialized = false;
	[SerializeField] [ReadOnly] private CrafterCore _crafter;
	[SerializeField] private TextMeshProUGUI _itemName;
	[SerializeField] private Image _itemImage;
	[SerializeField] private TextMeshProUGUI _descriptionText;
	[SerializeField] private Transform _ingredientList;
	[SerializeField] private GameObject _ingredientPrefab;

	public void Initialize(CrafterCore crafter) {
		if (Initialized) return;
		ResetInfo();
		_crafter = crafter;
		Initialized = true;
	}

	public void SetRecipe(RecipeData data) {
		if (data == null) {
			ResetInfo();
			return;
		}

		SetItemInfo(data.ResultSetList.List[0].ItemData);
		RemoveAllChildrenOfList();
		foreach (ItemSet ingredient in data.IngredientSetList.List) {
			IngredientIcon ingredientIcon = Instantiate(_ingredientPrefab, _ingredientList).GetComponent<IngredientIcon>();
			ingredientIcon.SetItem(ingredient.ItemData, ingredient.ItemCount * _crafter.SelectedCraftCount);
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