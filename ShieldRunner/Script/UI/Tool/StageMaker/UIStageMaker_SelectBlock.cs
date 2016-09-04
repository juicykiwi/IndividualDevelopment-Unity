using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIStageMaker_SelectBlock : MonoBehaviour
{
	[SerializeField]
	InputField _inputFieldBlockId = null;

	// Method

	public void OnEditEndBlockId()
	{
		string text = _inputFieldBlockId.text;
		StageMakerSceneControl.instance.SelectBlockId = Convert.ToInt32(text);
	}
}
