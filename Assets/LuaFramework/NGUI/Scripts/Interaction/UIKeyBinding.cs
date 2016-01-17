//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// This class makes it possible to activate or select something by pressing a key (such as space bar for example).
/// </summary>

[AddComponentMenu("NGUI/Interaction/Key Binding")]
public class UIKeyBinding : MonoBehaviour
{
	public enum Action
	{
		PressAndClick,
		Select,
		All,
	}

	public enum Modifier
	{
		Any,
		Shift,
		Control,
		Alt,
		None,
	}

	/// <summary>
	/// Key that will trigger the binding.
	/// </summary>

	public KeyCode keyCode = KeyCode.None;

	/// <summary>
	/// Modifier key that must be active in order for the binding to trigger.
	/// </summary>

	public Modifier modifier = Modifier.Any;

	/// <summary>
	/// Action to take with the specified key.
	/// </summary>

	public Action action = Action.PressAndClick;

	bool mIgnoreUp = false;
	bool mIsInput = false;
	bool mPress = false;

	/// <summary>
	/// If we're bound to an input field, subscribe to its Submit notification.
	/// </summary>

	protected virtual void Start ()
	{
		UIInput input = GetComponent<UIInput>();
		mIsInput = (input != null);
		if (input != null) EventDelegate.Add(input.onSubmit, OnSubmit);
	}

	/// <summary>
	/// Ignore the KeyUp message if the input field "ate" it.
	/// </summary>

	protected virtual void OnSubmit () { if (UICamera.currentKey == keyCode && IsModifierActive()) mIgnoreUp = true; }

	/// <summary>
	/// Convenience function that checks whether the required modifier key is active.
	/// </summary>

	protected virtual bool IsModifierActive ()
	{
		if (modifier == Modifier.Any) return true;

		if (modifier == Modifier.Alt)
		{
			if (Input.GetKey(KeyCode.LeftAlt) ||
				Input.GetKey(KeyCode.RightAlt)) return true;
		}
		else if (modifier == Modifier.Control)
		{
			if (Input.GetKey(KeyCode.LeftControl) ||
				Input.GetKey(KeyCode.RightControl)) return true;
		}
		else if (modifier == Modifier.Shift)
		{
			if (Input.GetKey(KeyCode.LeftShift) ||
				Input.GetKey(KeyCode.RightShift)) return true;
		}
		else if (modifier == Modifier.None)
			return
				!Input.GetKey(KeyCode.LeftAlt) &&
				!Input.GetKey(KeyCode.RightAlt) &&
				!Input.GetKey(KeyCode.LeftControl) &&
				!Input.GetKey(KeyCode.RightControl) &&
				!Input.GetKey(KeyCode.LeftShift) &&
				!Input.GetKey(KeyCode.RightShift);
		return false;
	}

	/// <summary>
	/// Process the key binding.
	/// </summary>

	protected virtual void Update ()
	{
		if (UICamera.inputHasFocus) return;
		if (keyCode == KeyCode.None || !IsModifierActive()) return;

		bool keyDown = Input.GetKeyDown(keyCode);
		bool keyUp = Input.GetKeyUp(keyCode);

		if (keyDown) mPress = true;

		if (action == Action.PressAndClick || action == Action.All)
		{
			if (keyDown)
			{
				UICamera.currentTouch = UICamera.controller;
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.currentTouch.current = gameObject;
				OnBindingPress(true);
				UICamera.currentTouch.current = null;
			}

			if (mPress && keyUp)
			{
				UICamera.currentTouch = UICamera.controller;
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.currentTouch.current = gameObject;
				OnBindingPress(false);
				OnBindingClick();
				UICamera.currentTouch.current = null;
			}
		}

		if (action == Action.Select || action == Action.All)
		{
			if (keyUp)
			{
				if (mIsInput)
				{
					if (!mIgnoreUp && !UICamera.inputHasFocus)
					{
						if (mPress) UICamera.selectedObject = gameObject;
					}
					mIgnoreUp = false;
				}
				else if (mPress) UICamera.selectedObject = gameObject;
			}
		}

		if (keyUp) mPress = false;
	}

	protected virtual void OnBindingPress (bool pressed) { UICamera.Notify(gameObject, "OnPress", pressed); }
	protected virtual void OnBindingClick () { UICamera.Notify(gameObject, "OnClick", null); }
}
