using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate List<object> DelGetDataList();
public delegate void DelDebugLogData(object obj);

public class XmlSerializerData {

	// Property

	public int _id = 0;

	// Method

	public static void XmlDebugLogData(object obj)
	{
		XmlSerializerData data = obj as XmlSerializerData;
		if (data == null)
			return;

		data.DebugLogDataInfo();
	}

	public static string GetPath()
	{
		return "XmlData/";
	}

	public virtual void DebugLogDataInfo()
	{
//		Debug.LogFormat("intData : {0}", this._id);
	}
}
