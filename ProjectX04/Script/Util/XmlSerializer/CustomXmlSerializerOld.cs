using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace CustomXmlSerializerUtil
{
	public class CustomXmlSerializerOld {

		bool _isOutputLog = false;

		// Method

		static CustomXmlSerializerOld _instance = null;

		public static CustomXmlSerializerOld instance 
		{
			get
			{
				if (_instance == null)
				{
					_instance = new CustomXmlSerializerOld();
					_instance.Init();
				}

				return _instance;
			}
		}

		// Property

		public UnityEngine.Object dataObj= null;

		// Method

		public void Init () {
		}

		public void SaveEmptyData(UnityEngine.Object dataObj)
		{
			if (dataObj == null)
				return;

			System.Type type = System.Type.GetType(dataObj.name);
			string xmlEmptySavePath = PathHelper.instance.GetXmlEmptySavePath();
			string path = xmlEmptySavePath + type.Name + ".xml";
			Debug.LogFormat("XmlEmptySavePath Path : {0}", path);

			if (Directory.Exists(xmlEmptySavePath) == false)
			{
				Directory.CreateDirectory(xmlEmptySavePath);
			}

			List<object> saveDataList = new List<object>();

			var saveEmptyData = Activator.CreateInstance(type);
			saveDataList.Add(saveEmptyData);
			saveDataList.Add(saveEmptyData);

			Type[] extraType = { type };
			
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<object>), extraType);
			
			FileStream stream = new FileStream(path, FileMode.Create);
			xmlSerializer.Serialize(stream, saveDataList);
			stream.Close();
		}

//		public void LoadData(UnityEngine.Object obj)
//		{
//			if (obj == null)
//				return;
//			
//			System.Type type = System.Type.GetType(obj.name);
//			string loadSavePath = PathHelper.instance.GetXmlSaveLoadPath(type);
//			string path = loadSavePath + type.Name + ".xml";
//			Debug.LogFormat("LoadData Path : {0}", path);
//			
//			if (Directory.Exists(loadSavePath) == false)
//			{
//				Debug.LogError("CustomXmlSerializerOld::LoadData : Not exist diretory.");
//				return;
//			}
//			
//			if (File.Exists(path) == false)
//			{
//				Debug.LogError("CustomXmlSerializerOld::LoadData : Not exist file.");
//				return;
//			}
//			
//			List<object> loadDataList = new List<object>();
//			
//			Type[] extraType = { type };
//			
//			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<object>), extraType);
//			
//			/*
//			 * FileStream 사용 시 encoding=“ks_c_56011987”에서 에러 발생한다.
//			 * 그래서 Deserialize 시에는 아래 주석 방법 대신 StreamReader를 사용한다.
//			 * 
//			 * FileStream stream = new FileStream(path, FileMode.Open);
//			 * testInfoDataList = (List<ChaStatBase>)xmlSerializer.Deserialize(stream);
//			 * stream.Close();
//			 */
//			
//			StreamReader file = new StreamReader(path);
//			loadDataList = (List<object>)xmlSerializer.Deserialize(file);
//			
//			foreach (object data in loadDataList)
//			{
//				BindingFlags bingingFlags = BindingFlags.Public | BindingFlags.Static;
//				MethodInfo methodInfoDebug = type.BaseType.GetMethod("XmlDebugLogData", bingingFlags);
//				if (methodInfoDebug == null)
//					return;
//				
//				DelDebugLogData delDebugLogData = (DelDebugLogData)System.Delegate.CreateDelegate(typeof(DelDebugLogData), methodInfoDebug);
//				delDebugLogData(data);
//			}
//		}

		public void SaveData(List<object> dataList, System.Type objType, string postfix)
		{
			string loadSavePath = PathHelper.instance.GetXmlSaveLoadPath(objType);
			string path = loadSavePath + PathHelper.instance.GetXmlSaveLoadFileName(objType, postfix);
			Debug.LogFormat("XmlDataPath Path : {0}", path);

			if (Directory.Exists(loadSavePath) == false)
			{
				Directory.CreateDirectory(loadSavePath);
			}
			
			Type[] extraType = { objType };
			
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<object>), extraType);
			
			FileStream stream = new FileStream(path, FileMode.Create);
			xmlSerializer.Serialize(stream, dataList);
			stream.Close();
		}

		public List<object> LoadData(System.Type objType, string postfix)
		{
			System.Type type = objType;
			string loadSavePath = PathHelper.instance.GetXmlSaveLoadPath(type);
			string path = loadSavePath + PathHelper.instance.GetXmlSaveLoadFileName(type, postfix);
			Debug.LogFormat("LoadData Path : {0}", path);

			if (Directory.Exists(loadSavePath) == false)
			{
				Debug.LogError("CustomXmlSerializerOld::LoadData : Not exist diretory.");
				return null;
			}
			
			if (File.Exists(path) == false)
			{
				Debug.LogError("CustomXmlSerializerOld::LoadData : Not exist file.");
				return null;
			}

			List<object> loadDataList = new List<object>();
			
			Type[] extraType = { type };
			
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<object>), extraType);
			
			/*
			 * FileStream 사용 시 encoding=“ks_c_56011987”에서 에러 발생한다.
			 * 그래서 Deserialize 시에는 아래 주석 방법 대신 StreamReader를 사용한다.
			 * 
			 * FileStream stream = new FileStream(path, FileMode.Open);
			 * testInfoDataList = (List<ChaStatBase>)xmlSerializer.Deserialize(stream);
			 * stream.Close();
			 */
			
			StreamReader file = new StreamReader(path);
			loadDataList = (List<object>)xmlSerializer.Deserialize(file);
			file.Close();

			if (_isOutputLog == true)
			{
				foreach (object data in loadDataList)
				{
					BindingFlags bingingFlags = BindingFlags.Public | BindingFlags.Static;
					MethodInfo methodInfoDebug = type.BaseType.GetMethod("XmlDebugLogData", bingingFlags);
					if (methodInfoDebug == null)
						continue;

					DelDebugLogData delDebugLogData = (DelDebugLogData)System.Delegate.CreateDelegate(typeof(DelDebugLogData), methodInfoDebug);
					delDebugLogData(data);
				}
			}

			return loadDataList;
		}

		// Resources Data

		public List<object> LoadDataInResources(System.Type objType, string path , string postfix)
		{
			System.Type type = objType;
			Type[] extraType = { type };

			List<object> loadDataList = new List<object>();
			
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<object>), extraType);

			/*
			 * StreamReader 방식에서 Resources.Load<TextAsset>() 이용하는 방식으로 변경
			 * 
			 * Unity 빌드 시 Resources를 자동으로 복사하는데
			 * StreamReader 밭식에서의 DataPath를 정상적으로 이용하지 못하는 것 같다.
			 * 
			 * Path에 크게 구여받지 않는 Resources.Load() 함수를 사용하면서
			 * 간편하게 이용이 가능하게 되었다.
			 */

			string loadDataName = path + PathHelper.instance.GetXmlSaveLoadFileName(objType, postfix, false);

			TextAsset textAsset = Resources.Load<TextAsset>(loadDataName);
			if (textAsset == null)
			{
				Debug.LogError("CustomXmlSerializerOld::LoadDataEx : Not exist text file.");
				return null;
			}

			using (System.IO.StringReader reader = new System.IO.StringReader(textAsset.text))
			{
				loadDataList = xmlSerializer.Deserialize(reader) as List<object>;
			}

			if (_isOutputLog == true)
			{
				foreach (object data in loadDataList)
				{
					BindingFlags bingingFlags = BindingFlags.Public | BindingFlags.Static;
					MethodInfo methodInfoDebug = type.BaseType.GetMethod("XmlDebugLogData", bingingFlags);
					if (methodInfoDebug == null)
						continue;
					
					DelDebugLogData delDebugLogData = (DelDebugLogData)System.Delegate.CreateDelegate(typeof(DelDebugLogData), methodInfoDebug);
					delDebugLogData(data);
				}
			}
			
			return loadDataList;
		}

		// Persistent Data

		public void SavePersistentData(List<object> dataList, System.Type objType, string path, string postfix)
		{
			string persistenPath = PathHelper.instance.GetXmlPersistentDataPath(objType);
			string fileName = PathHelper.instance.GetXmlSaveLoadFileName(objType, postfix);

			string totalPath = persistenPath + path + fileName;
			
			if (Directory.Exists(persistenPath + path) == false)
			{
				Directory.CreateDirectory(totalPath);
			}
			
			Type[] extraType = { objType };
			
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<object>), extraType);
			
			FileStream stream = new FileStream(totalPath, FileMode.Create);
			xmlSerializer.Serialize(stream, dataList);
			stream.Close();
		}

		public List<object> LoadPersistentData(System.Type objType, string path , string postfix)
		{
			string persistenPath = PathHelper.instance.GetXmlPersistentDataPath(objType);
			string fileName = PathHelper.instance.GetXmlSaveLoadFileName(objType, postfix);

			string totalPath = persistenPath + path + fileName;

			/* Persistent path
			 * 
			 * Windows path ex : 
			 * 
			 * 
			 * Mac path ex :
			 * /Users/joen0622/Library/Application Support/June Flower/ProjectX04/UserInfoData.xml
			 * 
			 * Android path ex :
			 * 
			 * 
			 * iOS path ex :
			 * 
			 */

			if (File.Exists (totalPath) == false)
				return null;

			Type[] extraType = { objType };
			List<object> loadDataList = new List<object>();
			
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<object>), extraType);

			StreamReader file = new StreamReader(totalPath);
			loadDataList = (List<object>)xmlSerializer.Deserialize(file);
			file.Close();

			if (_isOutputLog == true)
			{
				foreach (object data in loadDataList)
				{
					BindingFlags bingingFlags = BindingFlags.Public | BindingFlags.Static;
					MethodInfo methodInfoDebug = objType.BaseType.GetMethod("XmlDebugLogData", bingingFlags);
					if (methodInfoDebug == null)
						continue;
					
					DelDebugLogData delDebugLogData = (DelDebugLogData)System.Delegate.CreateDelegate(typeof(DelDebugLogData), methodInfoDebug);
					delDebugLogData(data);
				}
			}
			
			return loadDataList;
		}
	}
}