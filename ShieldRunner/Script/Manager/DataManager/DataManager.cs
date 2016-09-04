using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CustomXmlSerializerUtil;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum DataLoadType
{
    Resources,
    Persistent,
}

public abstract class DataManager<T, I>
    : Singleton<I> where T : class, new() where I : MonoBehaviour
{
	[SerializeField]
	bool _isLoadData = false;
	public bool IsLoadData { get { return _isLoadData; } }

    [SerializeField]
    protected List<T> _dataList = new List<T>();

    public IEnumerator<T> DataListEnumerator { get { return _dataList.GetEnumerator(); } }

	// Method

    public abstract void LoadData();
    public abstract void SaveData();

	public void Init()
	{
		if (_isLoadData == false)
		{
			LoadData();
			
			_isLoadData = true;
		}
	}

    public virtual void ClearData()
    {
        _dataList.Clear();
    }

    protected void LoadData(string fileName, DataLoadType dataLoadType)
    {
        List<T> dataList = null;
        switch (dataLoadType)
        {
            case DataLoadType.Persistent:
                {
                    dataList = CustomXmlSerializer.instance.LoadPersistentData<T>(fileName + ".xml");
                }
                break;

            case DataLoadType.Resources:
                {
                    dataList = CustomXmlSerializer.instance.LoadDataInResources<T>(fileName);
                }
                break;
        }

        if (dataList == null)
            return;

        _dataList.AddRange(dataList);
    }

    protected void LoadOldData()
    {
        Type dataType = typeof(T);

        string path = CustomXmlSerializer.GetDataPath(dataType);

        List<object> oldDataList = 
            CustomXmlSerializerOld.instance.LoadPersistentData(dataType, path, "");

        if (oldDataList.Count <= 0)
            return;

        for (int index = 0; index < oldDataList.Count; ++index)
        {
            T infoData = oldDataList[index] as T;
            if (infoData == null)
                continue;

            _dataList.Add(infoData);
        }
    }

    public void LoadDataAll()
    {
        _dataList.Clear();

        List<T> uDataList = CustomXmlSerializer.instance.LoadDataAllInResources<T>();
        if (uDataList == null)
            return;

        _dataList.AddRange(uDataList);
    }

    protected void SaveData(string fileName, DataLoadType dataLoadType)
    {
        switch (dataLoadType)
        {
            case DataLoadType.Persistent:
                {
                    CustomXmlSerializer.instance.SavePersistentData<T>(_dataList, fileName + ".xml");
                }
                break;

            case DataLoadType.Resources:
                {
                    CustomXmlSerializer.instance.SaveData<T>(_dataList, fileName + ".xml");
                }
                break;
        }

        RefreshAssetData();
    }

	public void RefreshAssetData()
	{
#if UNITY_EDITOR
		AssetDatabase.Refresh();
#endif
	}
}

#if UNITY_EDITOR

[CustomEditor(typeof(DataManager<,>))]
public class DataManagerEditor<T, I>
    : Editor where T : class, new() where I : MonoBehaviour
{
    DataManager<T, I> _target = null;

	public override void OnInspectorGUI ()
	{
		_target = (DataManager<T, I>)target;
		
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
					_target.LoadData();
				}
				
				if (GUILayout.Button("Save") == true)
				{
					_target.SaveData();
				}
				
				if (GUILayout.Button("Clear") == true)
				{
					_target.ClearData();
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}
}

#endif