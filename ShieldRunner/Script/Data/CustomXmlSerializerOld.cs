using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace CustomXmlSerializerUtil
{
	public class CustomXmlSerializerOld : SingletonNew<CustomXmlSerializerOld>
	{
        // Method

		public void SaveEmptyData(UnityEngine.Object dataObj, string path)
		{
			if (dataObj == null)
				return;

			System.Type type = System.Type.GetType(dataObj.name);

			string resourcesPath = PathHelper.instance.GetResourcesFolderPath();

			string saveDataPath = resourcesPath + path;
			string saveDataName = saveDataPath + type.Name + ".xml";
			Debug.LogFormat("XmlEmptySavePath Path : {0}", saveDataPath);

			if (Directory.Exists(saveDataPath) == false)
			{
				Directory.CreateDirectory(saveDataPath);
			}

			List<object> saveDataList = new List<object>();

			var saveEmptyData = Activator.CreateInstance(type);
			saveDataList.Add(saveEmptyData);

			Type[] extraType = { type };
			
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<object>), extraType);
			
			FileStream stream = new FileStream(saveDataName, FileMode.Create);
			xmlSerializer.Serialize(stream, saveDataList);
			stream.Close();
		}

		public void SaveData(List<object> dataList, System.Type objType, string path, string postfix)
		{
			string resourcesPath = PathHelper.instance.GetResourcesFolderPath();

			string saveDataPath = resourcesPath + path;
			string saveDataName = saveDataPath + PathHelper.instance.GetXmlSaveLoadFileName(objType, postfix);
			Debug.LogFormat("XmlDataPath Path : {0}", saveDataPath);

			if (Directory.Exists(saveDataPath) == false)
			{
				Directory.CreateDirectory(saveDataPath);
			}
			
			Type[] extraType = { objType };
			
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<object>), extraType);
			
			FileStream stream = new FileStream(saveDataName, FileMode.Create);
			xmlSerializer.Serialize(stream, dataList);
			stream.Close();
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
				Debug.LogWarning("Fail LoadDataInResources. Not exist text file.");
				return null;
			}

			using (System.IO.StringReader reader = new System.IO.StringReader(textAsset.text))
			{
				loadDataList = xmlSerializer.Deserialize(reader) as List<object>;
			}
			
			return loadDataList;
		}

        public List<T> LoadDataAllInResources<T>(System.Type objType, string path)
        {
            List<T> loadedList = new List<T>();

            TextAsset[] textAssets = Resources.LoadAll<TextAsset>(path);
            if (textAssets.Length <= 0)
            {
                Debug.LogWarning("Fail LoadDataAllInResources. Not exist text file.");
                return null;
            }

            foreach (TextAsset textAsset in textAssets)
            {
                List<object> objectList = DeserializeTextAsset(objType, textAsset);
                if (objectList == null)
                    continue;

                foreach (object loadedObject in objectList)
                {
                    if ((loadedObject is T) == false)
                        continue;

                    loadedList.Add((T)loadedObject);
                }
            }

            return loadedList;
        }

        List<object> DeserializeTextAsset(System.Type objType, TextAsset textAsset)
        {
            System.Type type = objType;
            Type[] extraType = { type };

            List<object> loadDataList = new List<object>();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<object>), extraType);

            using (System.IO.StringReader reader = new System.IO.StringReader(textAsset.text))
            {
                loadDataList = xmlSerializer.Deserialize(reader) as List<object>;
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
			
			return loadDataList;
		}
	}
}