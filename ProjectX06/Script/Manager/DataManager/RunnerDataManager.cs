using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class RunnerDataManager : DataManager<RunnerDataManager>
{
    [SerializeField]
    RunnerDataList _runnerDataList = new RunnerDataList();

    public PrefabDataDict<Runner> _prefabDataDict = 
        new PrefabDataDict<Runner>();


    public override void ClearData()
    {
        _runnerDataList.Clear();
    }

    public override void LoadData()
    {
        ClearData();

        _runnerDataList.LoadDataAll();
        _prefabDataDict.LoadPrefabAll();
    }

    public void LoadDataById(int id)
    {
        ClearData();
        _runnerDataList.LoadData(id.ToString("D3"));
    }

    public override void SaveData()
    {
    }

    public void SaveDataById(int id)
    {
        if (_runnerDataList.Count <= 0)
            return;
        
        string saveName = id.ToString("D3");
        _runnerDataList.SaveData(id.ToString(saveName));

        RefreshAssetData();
    }

    string DataNameById(int id)
    {
        return typeof(RunnerData).Name + id.ToString("D3");
    }

    public RunnerData GetRunnerDataById(int id)
    {
        return _runnerDataList.FindDatabyId(id);
    }

    public List<RunnerData>.Enumerator GetRunnerDataListEnumerator()
    {
        return _runnerDataList.GetList().GetEnumerator();
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(RunnerDataManager))]
public class RunnerDataManagerEditor : DataManagerEditor<RunnerDataManager>
{
    RunnerDataManager _runnerDataManager = null;

    int _selectRunnerId = 0;


    public override void OnInspectorGUI()
    {
        _runnerDataManager = target as RunnerDataManager;

        _selectRunnerId = EditorGUILayout.IntField("Editor runner ID : ", _selectRunnerId);
        GUILayout.Space(20f);

        base.OnInspectorGUI();
    }

    protected override void OnLoadButton()
    {
        _runnerDataManager.LoadDataById(_selectRunnerId);
    }

    protected override void OnSaveButton()
    {
        _runnerDataManager.SaveDataById(_selectRunnerId);
    }
}

#endif
