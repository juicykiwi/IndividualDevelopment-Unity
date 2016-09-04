using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIStageMaker_SaveLoad : MonoBehaviour
{
	[SerializeField]
	InputField _inputFieldStageId = null;

	[SerializeField]
	int _selectedStageId = 0;
	
	// Method
	
	public void OnEditEndStageId()
	{
		string text = _inputFieldStageId.text;
		
		_selectedStageId = Convert.ToInt32(text);
	}
	
	public void OnLoadButton()
	{
		StageMakerSceneControl.instance.LoadStage(_selectedStageId);
	}
	
	public void OnSaveButton()
	{
		StageMakerSceneControl.instance.SaveStage(_selectedStageId);
	}
}
