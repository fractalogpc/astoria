using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrafterView : MonoBehaviour
{
	public delegate void RecipeSelected(RecipeData recipe);
	public event RecipeSelected OnRecipeSelected;
	public delegate void CraftCountUp();
	public event CraftCountUp OnCraftCountUp;
	public delegate void CraftCountDown();
	public event CraftCountDown OnCraftCountDown;
	public delegate void CraftButtonClicked();
	public event CraftButtonClicked OnCraftButtonClicked;
	
	[Header("Item Info")]
	[SerializeField] private TextMeshProUGUI _nameText;
	[SerializeField] private Image _iconImage;
	[SerializeField] private TextMeshProUGUI _descriptionText;
	[Header("Ingredient List")]
	[SerializeField] private Transform _ingredientListParent;
	[Header("List/Queue")]
	[SerializeField] private GameObject _iconAndCountPrefab;
	[Header("Recipe Queue")]
	[SerializeField] private Transform _queueParent;
	[Header("Recipe Grid")]
	[SerializeField] private Transform _recipeGridParent;
	[SerializeField] private GameObject _recipePrefab;
	[Header("Crafting Count")]
	[SerializeField] private Button _increaseButton;
	[SerializeField] private Button _decreaseButton;
	[SerializeField] private TextMeshProUGUI _countText;
	[Header("Crafting")]
	[SerializeField] private Button _craftButton;
	
	public void SetRecipeInfo(RecipeData data) {
		if (data == null) {
			_nameText.text = "";
			_iconImage.sprite = null;
			_descriptionText.text = "";
			return;
		}
		_nameText.text = data.Result.ItemName;
		_iconImage.sprite = data.Result.ItemIcon;
		_descriptionText.text = data.Result.ItemDescription;
	}
	
	public void SetAvailableRecipeList(List<RecipeData> recipes) {
		foreach (Transform child in _recipeGridParent) {
			Destroy(child.gameObject);
		}
		foreach (RecipeData recipe in recipes) {
			SelectableRecipe recipeSelectable = Instantiate(_recipePrefab, _recipeGridParent).GetComponent<SelectableRecipe>();
			recipeSelectable.Initialize(recipe);
			recipeSelectable.RegisterListener(OnRecipeClicked);
		}
	}
	
	public void SetRecipeDisabled(RecipeData recipe, bool disabled) {
		foreach (Transform child in _recipeGridParent) {
			SelectableRecipe recipeSelectable = child.GetComponent<SelectableRecipe>();
			if (recipeSelectable != null && recipeSelectable.Recipe == recipe) {
				recipeSelectable.SetDisabledLook(disabled);
			}
		}
	}
	
	public void SetIngredientList(ItemSetList ingredientSetList) {
		foreach (Transform child in _ingredientListParent) {
			Destroy(child.gameObject);
		}
		foreach (ItemSet ingredient in ingredientSetList.List) {
			IconAndCount iconAndCount = Instantiate(_iconAndCountPrefab, _ingredientListParent).GetComponent<IconAndCount>();
			iconAndCount.SetIconAndCount(ingredient.ItemData, ingredient.ItemCount);
		}
	}
	
	public void SetCraftQueue(List<CraftingSession> craftQueue) {
		foreach (Transform child in _queueParent) {
			Destroy(child.gameObject);
		}
		foreach (CraftingSession session in craftQueue) {
			IconAndCount iconAndCount = Instantiate(_iconAndCountPrefab, _queueParent).GetComponent<IconAndCount>();
			iconAndCount.SetIconAndCount(session.Recipe.Result, session.CraftCount);
		}
	}
	
	public void SetCraftCountUpInteractable(bool interactable) {
		_increaseButton.interactable = interactable;
	}
	
	public void SetCraftCountDownInteractable(bool interactable) {
		_decreaseButton.interactable = interactable;
	}
	
	public void SetCraftCount(int count) {
		_countText.text = count.ToString();
	}
	
	public void SetCraftButtonInteractable(bool interactable) {
		_craftButton.interactable = interactable;
		_craftButton.GetComponentInChildren<TextMeshProUGUI>().text = interactable ? "Start Craft" : "Can't Craft";
		_countText.GetComponentInChildren<TextMeshProUGUI>().text = interactable ? _countText.text : "X";
	}

	private void Start() {
		_craftButton.onClick.AddListener(OnCraftClicked);
		_increaseButton.onClick.AddListener(OnIncreaseClicked);
		_decreaseButton.onClick.AddListener(OnDecreaseClicked);
		_iconImage.preserveAspect = true;
	}

	private void OnRecipeClicked(RecipeData recipe) {
		OnRecipeSelected?.Invoke(recipe);	
	}
	
	private void OnCraftClicked() {
		OnCraftButtonClicked?.Invoke();
	}
	
	private void OnIncreaseClicked() {
		OnCraftCountUp?.Invoke();
	}
	
	private void OnDecreaseClicked() {
		OnCraftCountDown?.Invoke();
	}
}