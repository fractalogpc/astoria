using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
	public delegate void ElementSelected(RadialMenuElement element);
	public static event ElementSelected OnElementSelected;
	
	[Serializable]
	public class RadialMenuElement
	{
		public string Name;
		public string Description;
		public Sprite Icon;
		public string ID;
	}
	
	[SerializeField] private List<RadialMenuElement> _elements;
	[SerializeField] private RectTransform _radialBackground;
	[SerializeField] private TextMeshProUGUI _nameText;
	[SerializeField] private TextMeshProUGUI _descriptionText;
}
