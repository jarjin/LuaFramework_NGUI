using UnityEngine;

/// <summary>
/// Simple script used by Tutorial 11 that sets the color of the sprite based on the string value.
/// </summary>

[ExecuteInEditMode]
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/Examples/Set Color on Selection")]
public class SetColorOnSelection : MonoBehaviour
{
	UIWidget mWidget;

	public void SetSpriteBySelection ()
	{
		if (UIPopupList.current == null) return;
		if (mWidget == null) mWidget = GetComponent<UIWidget>();

		switch (UIPopupList.current.value)
		{
			case "White":	mWidget.color = Color.white;	break;
			case "Red":		mWidget.color = Color.red;		break;
			case "Green":	mWidget.color = Color.green;	break;
			case "Blue":	mWidget.color = Color.blue;		break;
			case "Yellow":	mWidget.color = Color.yellow;	break;
			case "Cyan":	mWidget.color = Color.cyan;		break;
			case "Magenta": mWidget.color = Color.magenta;	break;
		}
	}
}
