//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Attaching this script to a widget makes it react to key events such as tab, up, down, etc.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Key Navigation")]
public class UIKeyNavigation : MonoBehaviour
{
	/// <summary>
	/// List of all the active UINavigation components.
	/// </summary>

	static public BetterList<UIKeyNavigation> list = new BetterList<UIKeyNavigation>();

	public enum Constraint
	{
		None,
		Vertical,
		Horizontal,
		Explicit,
	}

	/// <summary>
	/// If a selection target is not set, the target can be determined automatically, restricted by this constraint.
	/// 'None' means free movement on both horizontal and vertical axis. 'Explicit' means the automatic logic will
	/// not execute, and only the explicitly set values will be used.
	/// </summary>

	public Constraint constraint = Constraint.None;

	/// <summary>
	/// Which object will be selected when the Up button is pressed.
	/// </summary>

	public GameObject onUp;

	/// <summary>
	/// Which object will be selected when the Down button is pressed.
	/// </summary>

	public GameObject onDown;

	/// <summary>
	/// Which object will be selected when the Left button is pressed.
	/// </summary>

	public GameObject onLeft;

	/// <summary>
	/// Which object will be selected when the Right button is pressed.
	/// </summary>

	public GameObject onRight;

	/// <summary>
	/// Which object will get selected on click.
	/// </summary>

	public GameObject onClick;

	/// <summary>
	/// Whether the object this script is attached to will get selected as soon as this script is enabled.
	/// </summary>

	public bool startsSelected = false;

	protected virtual void OnEnable ()
	{
		list.Add(this);

		if (startsSelected)
		{
#if UNITY_EDITOR
			if (!Application.isPlaying) return;
#endif
			if (UICamera.selectedObject == null || !NGUITools.GetActive(UICamera.selectedObject))
			{
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.selectedObject = gameObject;
			}
		}
	}

	protected virtual void OnDisable () { list.Remove(this); }

	protected GameObject GetLeft ()
	{
		if (NGUITools.GetActive(onLeft)) return onLeft;
		if (constraint == Constraint.Vertical || constraint == Constraint.Explicit) return null;
		return Get(Vector3.left, true);
	}

	GameObject GetRight ()
	{
		if (NGUITools.GetActive(onRight)) return onRight;
		if (constraint == Constraint.Vertical || constraint == Constraint.Explicit) return null;
		return Get(Vector3.right, true);
	}

	protected GameObject GetUp ()
	{
		if (NGUITools.GetActive(onUp)) return onUp;
		if (constraint == Constraint.Horizontal || constraint == Constraint.Explicit) return null;
		return Get(Vector3.up, false);
	}

	protected GameObject GetDown ()
	{
		if (NGUITools.GetActive(onDown)) return onDown;
		if (constraint == Constraint.Horizontal || constraint == Constraint.Explicit) return null;
		return Get(Vector3.down, false);
	}

	protected GameObject Get (Vector3 myDir, bool horizontal)
	{
		Transform t = transform;
		myDir = t.TransformDirection(myDir);
		Vector3 myCenter = GetCenter(gameObject);
		float min = float.MaxValue;
		GameObject go = null;

		for (int i = 0; i < list.size; ++i)
		{
			UIKeyNavigation nav = list[i];
			if (nav == this) continue;

			// Ignore disabled buttons
			UIButton btn = nav.GetComponent<UIButton>();
			if (btn != null && !btn.isEnabled) continue;

			// Reject objects that are not within a 45 degree angle of the desired direction
			Vector3 dir = GetCenter(nav.gameObject) - myCenter;
			float dot = Vector3.Dot(myDir, dir.normalized);
			if (dot < 0.707f) continue;

			// Exaggerate the movement in the undesired direction
			dir = t.InverseTransformDirection(dir);
			if (horizontal) dir.y *= 2f;
			else dir.x *= 2f;

			// Compare the distance
			float mag = dir.sqrMagnitude;
			if (mag > min) continue;
			go = nav.gameObject;
			min = mag;
		}
		return go;
	}

	static protected Vector3 GetCenter (GameObject go)
	{
		UIWidget w = go.GetComponent<UIWidget>();
		UICamera cam = UICamera.FindCameraForLayer(go.layer);

		if (cam != null)
		{
			Vector3 center = go.transform.position;

			if (w != null)
			{
				Vector3[] corners = w.worldCorners;
				center = (corners[0] + corners[2]) * 0.5f;
			}

			center = cam.cachedCamera.WorldToScreenPoint(center);
			center.z = 0;
			return center;
		}
		else if (w != null)
		{
			Vector3[] corners = w.worldCorners;
			return (corners[0] + corners[2]) * 0.5f;
		}
		return go.transform.position;
	}

	protected virtual void OnKey (KeyCode key)
	{
		if (!NGUITools.GetActive(this)) return;

		GameObject go = null;

		switch (key)
		{
		case KeyCode.LeftArrow:
			go = GetLeft();
			break;
		case KeyCode.RightArrow:
			go = GetRight();
			break;
		case KeyCode.UpArrow:
			go = GetUp();
			break;
		case KeyCode.DownArrow:
			go = GetDown();
			break;
		case KeyCode.Tab:
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				go = GetLeft();
				if (go == null) go = GetUp();
				if (go == null) go = GetDown();
				if (go == null) go = GetRight();
			}
			else
			{
				go = GetRight();
				if (go == null) go = GetDown();
				if (go == null) go = GetUp();
				if (go == null) go = GetLeft();
			}
			break;
		}

		if (go != null) UICamera.selectedObject = go;
	}

	protected virtual void OnClick ()
	{
		if (NGUITools.GetActive(this) && NGUITools.GetActive(onClick))
			UICamera.selectedObject = onClick;
	}
}
