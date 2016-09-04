using UnityEngine;
using System.Collections;

public class PathHelper {

	// Property

	static PathHelper _instance = null;
	public static PathHelper instance { 
		get { 
			if (_instance == null)
			{
				_instance = new PathHelper();
				_instance.Init();
			}

			return _instance; 
		} 
	}

	public string _assetDataPath = "";
	public string _persistentDataPath = "";

	string _xmlSaveLoadPath = "";
	string _xmlEmptySavePath = "";
	
	const string XmlDataPath = "Resources/XmlData/";
	const string XmlEmptyDataPath = "Resources/XmlData/EmptyData/";

	// Method

	public void Init()
	{
		_assetDataPath = Application.dataPath;
		_persistentDataPath = Application.persistentDataPath;

		_xmlSaveLoadPath = _assetDataPath + "/" + XmlDataPath;
		_xmlEmptySavePath = _assetDataPath + "/" + XmlEmptyDataPath;
	}

	public string GetXmlEmptySavePath()
	{
		return _xmlEmptySavePath;
	}

	public string GetXmlSaveLoadPath(System.Type dataType)
	{
		switch (dataType.ToString())
		{
		case "FieldInfoData":
		case "TileInfoData":
			return _xmlSaveLoadPath + "TileData/";

		default:
			break;
		}

		return _xmlSaveLoadPath;
	}

	public string GetXmlPersistentDataPath(System.Type dataType)
	{
		return _persistentDataPath;
	}

	public string GetXmlSaveLoadFileName(System.Type dataType, string postfix)
	{
		return GetXmlSaveLoadFileName(dataType, postfix, true);
	}

	public string GetXmlSaveLoadFileName(System.Type dataType, string postfix, bool isExtension)
	{
		if (isExtension == true)
			return dataType.ToString() + postfix + ".xml";
		else
			return dataType.ToString() + postfix;
	}
}
