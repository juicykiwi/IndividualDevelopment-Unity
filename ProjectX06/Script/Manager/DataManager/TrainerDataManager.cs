using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class TrainerDataManager : DataManager<TrainerDataManager>
{
    [SerializeField]
    TrainerDataList _trainerDataList = new TrainerDataList();

    Dictionary<int, List<TrainerData>> _trainerDataDictForLevel = 
        new Dictionary<int, List<TrainerData>>();

    public PrefabDataDict<Trainer> _prefabDataDict = 
        new PrefabDataDict<Trainer>();


    public override void ClearData()
    {
        _trainerDataList.Clear();
    }

    public override void LoadData()
    {
        ClearData();

        _trainerDataList.LoadDataAll();
        _prefabDataDict.LoadPrefabAll();

        GroupingTrainerDataForLevel();
    }

    public void LoadDataById(int id)
    {
        ClearData();
        _trainerDataList.LoadData(id.ToString("D3"));
    }

    public override void SaveData()
    {
    }

    public void SaveDataById(int id)
    {
        if (_trainerDataList.Count <= 0)
            return;

        string saveName = id.ToString("D3");
        _trainerDataList.SaveData(saveName);

        RefreshAssetData();
    }

    string DataNameById(int id)
    {
        return typeof(TrainerData).Name + id.ToString("D3");
    }

    void GroupingTrainerDataForLevel()
    {
        _trainerDataDictForLevel.Clear();

        for (int index = 0; index < _trainerDataList.Count; ++index)
        {
            TrainerData trainerData = _trainerDataList[index];
            if (trainerData == null)
                continue;

            if (_trainerDataDictForLevel.ContainsKey(trainerData._level) == false)
            {
                List<TrainerData> newTrainerDataList = new List<TrainerData>();
                _trainerDataDictForLevel.Add(trainerData._level, newTrainerDataList);
            }

            _trainerDataDictForLevel[trainerData._level].Add(trainerData);
        }
    }

    public TrainerData GetTrainerDataById(int id)
    {
        return _trainerDataList.FindDatabyId(id);
    }

    public TrainerData GetRandomTrainerDataByLevel(int level)
    {
        if (_trainerDataDictForLevel.ContainsKey(level) == false)
            return null;

        if (_trainerDataDictForLevel[level].Count <= 0)
            return null;
        
        int randomIndex = UnityEngine.Random.Range(0, _trainerDataDictForLevel[level].Count);
        return _trainerDataDictForLevel[level][randomIndex];
    }

    public List<TrainerData> GetTrainerDataList()
    {
        return _trainerDataList.GetList();
    }

    public List<TrainerData>.Enumerator GetTrainerDataListEnumerator()
    {
        return _trainerDataList.GetList().GetEnumerator();
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(TrainerDataManager))]
public class TrainerDataManagerEditor : DataManagerEditor<TrainerDataManager>
{
    TrainerDataManager _trainerDataManager = null;

    int _selectTrainerId = 0;


    public override void OnInspectorGUI()
    {
        _trainerDataManager = target as TrainerDataManager;

        _selectTrainerId = EditorGUILayout.IntField("Editor trainer ID : ", _selectTrainerId);
        GUILayout.Space(20f);

        base.OnInspectorGUI();
    }

    protected override void OnLoadButton()
    {
        _trainerDataManager.LoadDataById(_selectTrainerId);
    }

    protected override void OnSaveButton()
    {
        _trainerDataManager.SaveDataById(_selectTrainerId);
    }
}

#endif