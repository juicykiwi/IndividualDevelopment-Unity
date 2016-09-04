using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using CustomXmlSerializerUtil;

public delegate string DelGetPath();

public enum XmlDataPathType
{
	Normal,
	Persistent,
}

public class XmlInfoDataList<T> where T : XmlSerializerData
{
	public List<T> _infoDataList = new List<T>();
	
	public void Clear()
	{
		_infoDataList.Clear();
	}
	
	public void LoadInfoData()
	{
		LoadInfoData(XmlDataPathType.Normal, "");
	}
	
	public void LoadInfoData(XmlDataPathType pathType, string postFix)
	{
		System.Type infoDataType = System.Type.GetType(typeof(T).Name);
		
		BindingFlags bingingFlags = BindingFlags.Public | BindingFlags.Static;
		MethodInfo methodGetPath = infoDataType.GetMethod("GetPath", bingingFlags);
		if (methodGetPath == null)
		{
			methodGetPath = infoDataType.BaseType.GetMethod("GetPath", bingingFlags);
		}
		
		DelGetPath delGetPath = (DelGetPath)System.Delegate.CreateDelegate(typeof(DelGetPath), methodGetPath);
		string path = delGetPath();
		
		List<object> infoObjectList = null;
		if (pathType == XmlDataPathType.Normal)
		{
			infoObjectList = CustomXmlSerializerOld.instance.LoadDataInResources(infoDataType, path, postFix);
		}
		else if (pathType == XmlDataPathType.Persistent)
		{
			infoObjectList = CustomXmlSerializerOld.instance.LoadPersistentData(infoDataType, path, postFix);
		}
		
		if (infoObjectList == null)
			return;
		
		if (infoObjectList.Count <= 0)
			return;
		
		foreach (object infoObject in infoObjectList)
		{
			T tData = infoObject as T;
			if (tData == null)
				continue;
			
			_infoDataList.Add(tData);
		}
	}
	
	public void SaveInfoData(T infoData, XmlDataPathType pathType)
	{
		System.Type infoDataType = System.Type.GetType(typeof(T).Name);
		
		BindingFlags bingingFlags = BindingFlags.Public | BindingFlags.Static;
		MethodInfo methodGetPath = infoDataType.GetMethod("GetPath", bingingFlags);
		if (methodGetPath == null)
		{
			methodGetPath = infoDataType.BaseType.GetMethod("GetPath", bingingFlags);
		}
		
		DelGetPath delGetPath = (DelGetPath)System.Delegate.CreateDelegate(typeof(DelGetPath), methodGetPath);
		string path = delGetPath();
		
		List<object> infoObjectList = new List<object>();
		infoObjectList.Add(infoData);
		
		if (pathType == XmlDataPathType.Normal)
		{
			CustomXmlSerializerOld.instance.SaveData(infoObjectList, infoDataType, "");
		}
		else if (pathType == XmlDataPathType.Persistent)
		{
			CustomXmlSerializerOld.instance.SavePersistentData(infoObjectList, infoDataType, path, "");
		}
	}
	
	public List<T> GetInfoDataList()
	{
		return _infoDataList;
	}
	
	public T GetInfoDataById(int id)
	{
		T tfindedData = _infoDataList.Find(
			(T tData)=>{ return (tData._id == id); });

		return tfindedData;
	}
}
