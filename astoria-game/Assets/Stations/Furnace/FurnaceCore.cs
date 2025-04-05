using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceCore : MonoBehaviour
{
	public bool IsPowered {
		get;
		private set;
	}

	[SerializeField] private InventoryComponent _inputInv;
	[SerializeField] private InventoryComponent _fuelInv;
	[SerializeField] private InventoryComponent _outputInv;
	[SerializeField] private TextMeshProUGUI _statsText;
	[SerializeField] private Toggle _powerToggle;
	[SerializeField] private float _fuelUnitPerSec = 0.1f;
	[ReadOnly] private float _fuelUnits;

	private SmeltableInstance _currentSmeltable;
	private float _smeltTime;
	
	public void SetPower(bool on) {
		IsPowered = on;
		_powerToggle.isOn = on;
	}

	private void Start() {
		_powerToggle.onValueChanged.AddListener(SetPower);
	}

	private void Update() {
		// No smeltable
		if (_currentSmeltable == null) {
			_currentSmeltable = FirstSmeltItemInSmeltInv();
			if (_currentSmeltable == null) {
				_statsText.text = "Furnace Off";
				SetPower(false); 
			}
		}
		// Not running
		if (!IsPowered) {
			_statsText.text = "Furnace Off";
			return; 
		}
		if (_fuelUnits > 0) {
			_statsText.text = $"Item: {_currentSmeltable.ItemData.ItemName}\nProgress: {_currentSmeltable.FuelUsed/_currentSmeltable.ItemData.FuelCost}\nFuel: {_fuelUnits}";
			DecreaseFuelUnitsAndAddToUsage();
			if (_currentSmeltable.IsSmelted) {
				_outputInv.AddItemsByData(_currentSmeltable.GetResult());
				_inputInv.RemoveItem(_currentSmeltable);
				_currentSmeltable = null;
				if (_inputInv.GetItems().Count > 0) {
					_currentSmeltable = FirstSmeltItemInSmeltInv();
					return;
				}
				else {
					_statsText.text = "Furnace Off";
					SetPower(false); // No more items to smelt
					return;
				}
			}
		} // Fueled, move on
		else if (FuelItemsInFuelInv() > 0) {
			FuelInstance item = PopFirstFuelItemInFuelInv();
			_fuelUnits += item.ItemData.FuelValue;
		}
		else {
			_statsText.text = "Furnace Off";
			SetPower(false); // No fuel
		}
	}

	private void DecreaseFuelUnitsAndAddToUsage() {
		float usage = _fuelUnitPerSec * Time.deltaTime;
		_fuelUnits -= usage;
		if (_fuelUnits < 0) {
			usage -= Mathf.Abs(_fuelUnits); // Remove usage that went over
			_fuelUnits = 0;
		}
		_currentSmeltable.AddFuel(usage);
	}

	private int FuelItemsInFuelInv() {
		return _fuelInv.GetItemsOfDataType<FuelData>().Count;
	}
	
	private FuelInstance PopFirstFuelItemInFuelInv() {
		List<FuelInstance> output = _fuelInv.GetItemsOfDataType<FuelData>().Cast<FuelInstance>().ToList();
		if (output.Count <= 0) return null;
		_fuelInv.RemoveItem(output[0]);
		return output[0];
	}
	private SmeltableInstance FirstSmeltItemInSmeltInv() {
		List<SmeltableInstance> output = _inputInv.GetItemsOfDataType<SmeltableData>().Cast<SmeltableInstance>().ToList();
		return output.Count <= 0 ? null : output[0];
	}
}
