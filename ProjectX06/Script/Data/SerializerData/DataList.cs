using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CustomXmlSerializerUtil;

public delegate string DelGetPath();

[Serializable]
public class DataList<T>
{	
	[SerializeField]
	protected List<T> _dataList = new List<T>();
	
    public int Count { get { return _dataList.Count; } }

    public T this[int index] { get { return _dataList[index]; } }

	public List<T> GetList()
	{
        return _dataList;
	}

	public void Clear()
	{
        _dataList.Clear();
	}

    public void LoadData(string postFix, bool isPersistent = false)
    {
        string fileName = typeof(T).Name + postFix;

        List<T> tDataList = null;
        if (isPersistent == true)
        {
            tDataList = CustomXmlSerializer.instance.LoadPersistentData<T>(fileName + ".xml");
        }
        else
        {
            tDataList = CustomXmlSerializer.instance.LoadDataInResources<T>(fileName);
        }

        if (tDataList == null || tDataList.Count <= 0)
            return;

        _dataList.AddRange(tDataList);
    }

    public void LoadDataAll()
    {
        List<T> tDataList = CustomXmlSerializer.instance.LoadDataAllInResources<T>();
        if (tDataList == null || tDataList.Count <= 0)
            return;

        _dataList.AddRange(tDataList);
    }

    public void SaveData(string postFix, bool isPersistent = false)
    {
        string fileName = typeof(T).Name + postFix + ".xml";

        if (isPersistent == true)
        {
            CustomXmlSerializer.instance.SavePersistentData<T>(_dataList, fileName);
        }
        else
        {
            CustomXmlSerializer.instance.SaveData<T>(_dataList, fileName);
        }
    }
}
