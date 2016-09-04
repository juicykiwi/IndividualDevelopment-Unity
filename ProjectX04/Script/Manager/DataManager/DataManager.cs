using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CustomXmlSerializerUtil;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum DataStorageType
{
    Resources,
    Persistent,
}

public interface IDataManager
{
    void LoadData();
    void SaveData();
}

public class DataManager<T, U> : Singleton<T> where T : MonoBehaviour where U : class
{
    [SerializeField]
    protected List<U> _dataList = new List<U>();
    public List<U> DataList { get { return _dataList; } }

    public void ClearData()
    {
        _dataList.Clear();
    }

    protected void LoadData(string fileName, DataStorageType storageType)
    {
        List<U> uDataList = null;
        switch (storageType)
        {
            case DataStorageType.Persistent:
                {
                    uDataList = CustomXmlSerializer.instance.LoadPersistentData<U>(fileName + ".xml");
                }
                break;

            case DataStorageType.Resources:
                {
                    uDataList = CustomXmlSerializer.instance.LoadDataInResources<U>(fileName);
                }
                break;
        }

        if (uDataList == null)
            return;

        _dataList.AddRange(uDataList);
    }

    public void LoadAllData()
    {
        List<U> uDataList = CustomXmlSerializer.instance.LoadDataAllInResources<U>();
        if (uDataList == null)
            return;

        _dataList.AddRange(uDataList);
    }

    protected void SaveData(string fileName, DataStorageType storageType)
    {
        switch (storageType)
        {
            case DataStorageType.Persistent:
                {
                    CustomXmlSerializer.instance.SavePersistentData<U>(_dataList, fileName + ".xml");
                }
                break;

            case DataStorageType.Resources:
                {
                    CustomXmlSerializer.instance.SaveData<U>(_dataList, fileName + ".xml");
                }
                break;
        }
                
        #if UNITY_EDITOR
        AssetDatabase.Refresh();
        #endif
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(DataManager<,>))]
public abstract class DataManagerEditor<T, U> : Editor where T : MonoBehaviour where U : class
{
    protected DataManager<T, U> _target = null;


    public override void OnInspectorGUI()
    {
        _target = target as DataManager<T, U>;

        GUILayout.BeginVertical("Box", GUILayout.MaxHeight(10f));
        {
            GUILayout.Label("Command");

            GUILayout.BeginHorizontal();
            {
                View_ButtonBox();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();

        GUILayout.Space(20f);

        base.OnInspectorGUI();
    }

    protected abstract void View_ButtonBox();

    protected void View_ClearButton()
    {
        if (GUILayout.Button("Clear") == true)
        {
            _target.ClearData();
        }
    }

    protected void View_LoadButton()
    {
        if (GUILayout.Button("Load") == true)
        {
            IDataManager iDataManager = _target as IDataManager;
            if (iDataManager != null)
            {
                iDataManager.LoadData();
            }
        }
    }

    protected void View_LoadAllButton()
    {
        if (GUILayout.Button("LoadAll") == true)
        {
            _target.LoadAllData();
        }
    }

    protected void View_SaveButton()
    {
        if (GUILayout.Button("Save") == true)
        {
            IDataManager iDataManager = _target as IDataManager;
            if (iDataManager != null)
            {
                iDataManager.SaveData();
            }
        }
    }
}

#endif
