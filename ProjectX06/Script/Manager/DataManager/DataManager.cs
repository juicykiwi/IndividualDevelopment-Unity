using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class DataManager<T> : Singleton<T> where T : MonoBehaviour
{
	[SerializeField]
	bool _isLoadData = false;
	public bool IsLoadData { get { return _isLoadData; } }

	/**/

	public void Init()
	{
		if (_isLoadData == false)
		{
			LoadData();
			
			_isLoadData = true;
		}
	}

    public abstract void LoadData();
    public abstract void SaveData();
    public abstract void ClearData();

	public void RefreshAssetData()
	{
#if UNITY_EDITOR
		AssetDatabase.Refresh();
#endif
	}
}

#if UNITY_EDITOR

[CustomEditor(typeof(DataManager<>))]
public class DataManagerEditor<T> : Editor where T : MonoBehaviour
{
	protected DataManager<T> _target = null;

	public override void OnInspectorGUI ()
	{
		_target = (DataManager<T>)target;
		
		View_InfoDataCommand();
		
		GUILayout.Space(20f);
		
		base.OnInspectorGUI();
	}

	protected virtual void View_InfoDataCommand()
	{
		GUILayout.BeginVertical("Box", GUILayout.MaxHeight(10f));
		{
			GUILayout.Label("Command");
			
			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Load") == true)
				{
                    OnLoadButton();
				}
				
				if (GUILayout.Button("Save") == true)
				{
                    OnSaveButton();
				}
				
				if (GUILayout.Button("Clear") == true)
				{
                    OnClearButton();
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}

    protected virtual void OnLoadButton()
    {
        _target.LoadData();
    }

    protected virtual void OnSaveButton()
    {
        _target.SaveData();
    }

    protected virtual void OnClearButton()
    {
        _target.ClearData();
    }
}

#endif