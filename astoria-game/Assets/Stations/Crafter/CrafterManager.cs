using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Mirror;

[RequireComponent(typeof(CrafterView))]
public class CrafterManager : MonoBehaviour
{
	[SerializeField] private RecipeLibrary _recipeLibrary;
	[SerializeField] private CrafterView _view;
	
	private InventoryComponent _playerInventory;
	private CrafterModel _model;
	private CrafterPresenter _presenter;

	private void OnValidate() {
		_view = GetComponent<CrafterView>();
	}

	private void Start() {
		_playerInventory = PlayerInstance.Instance.GetComponentInChildren<InventoryComponent>();
		_model = new CrafterModel(_playerInventory, _recipeLibrary);
		_presenter = new CrafterPresenter(_model, _view);
	}

	private void Update() {
		_presenter.Update(Time.deltaTime);
	}
	
	private void OnDestroy() {
		_presenter.Dispose();
	}
}