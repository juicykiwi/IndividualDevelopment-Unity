using UnityEngine;
using System.Collections;

public class PathHelper {
	
	const string ResourcesPath = "Resources/";

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
	
	string _resourcesFolderPath = "";

    // Method

	public void Init()
	{
		_assetDataPath = Application.dataPath;
		_persistentDataPath = Application.persistentDataPath;

		_resourcesFolderPath = _assetDataPath + "/" + ResourcesPath;
	}

	public string GetResourcesFolderPath()
	{
		return _resourcesFolderPath;
	}

	public string GetXmlPersistentDataPath(System.Type dataType)
	{
		return _persistentDataPath + "/";
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
