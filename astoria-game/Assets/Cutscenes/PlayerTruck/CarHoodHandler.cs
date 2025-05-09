using UnityEngine;

public class CarHoodHandler : Interactable
{
	[SerializeField] private Animator _hoodAnimator;
	[SerializeField] private bool _isOpen = false;
	
	public override void Interact()
	{
		_isOpen = !_isOpen;
		_interactText = _isOpen ? "Close Hood" : "Open Hood";
		_hoodAnimator.SetBool("IsOpen", _isOpen);
	}
}
