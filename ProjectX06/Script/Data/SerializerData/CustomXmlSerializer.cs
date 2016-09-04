using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace CustomXmlSerializerUtil
{
    [AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class SerializeDataPathAttribute : Attribute
    {
        string _path = "";

        public SerializeDataPathAttribute(string path)
        {
            _path = path;
        }

        public string DataPath()
        {
            return _path;
        }
    }

	public class CustomXmlSerializer : SingletonNew<CustomXmlSerializer>
	{
        #region Save data

		public void SaveData<T>(List<T> dataList, string fileName)
		{
            SaveData(typeof(T), dataList as IList, fileName);
		}

        public void SaveData(Type type, IList dataList, string fileName)
        {
            string resourcesPath = Application.dataPath + "/Resources/";
            SaveData(type, dataList, fileName, resourcesPath);
        }

        public void SavePersistentData<T>(List<T> dataList, string fileName)
        {
            SavePersistentData(typeof(T), dataList as IList, fileName);
        }

        public void SavePersistentData(Type type, IList dataList, string fileName)
        {
            string persistentPath = Application.persistentDataPath + "/";
            SaveData(type, dataList, fileName, persistentPath);
        }

        void SaveData(Type type, IList dataList, string fileName, string path)
        {
            string dataPath = GetDataPath(type);
            string saveDataPath = path + dataPath;
            string saveDataName = saveDataPath + fileName;
            Debug.LogFormat("Xml save data path : {0}", saveDataName);

            if (Directory.Exists(saveDataPath) == false)
            {
                Directory.CreateDirectory(saveDataPath);
            }

            var genericType = typeof(List<>);
            var specificListType = genericType.MakeGenericType(type);
            XmlSerializer xmlSerializer = new XmlSerializer(specificListType);

            FileStream stream = new FileStream(saveDataName, FileMode.Create);
            {
                XmlWriterSettings setting = new XmlWriterSettings() {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                };

                XmlWriter writer = XmlWriter.Create(stream, setting);
                xmlSerializer.Serialize(writer, dataList);
            }
            stream.Close();
        }

        #endregion


        #region Load data

        public List<T> LoadDataInResources<T>(string fileName)
        {
            string dataPath = GetDataPath<T>();
            Debug.LogFormat("Xml load data in resources path : {0}", dataPath + fileName);

            TextAsset textAsset = Resources.Load<TextAsset>(dataPath + fileName);
            if (textAsset == null)
            {
                Debug.LogWarning("Fail LoadDataInResources. Not exist text file.");
                return null;
            }
                
            return DeserializeTextAsset<T>(textAsset);
        }

        public List<T> LoadDataAllInResources<T>()
        {
            string dataPath = GetDataPath<T>();
            Debug.LogFormat("Xml load data all in resources path : {0}", dataPath);

            TextAsset[] textAssets = Resources.LoadAll<TextAsset>(dataPath);
            if (textAssets.Length <= 0)
            {
                Debug.LogWarning("Fail LoadDataAllInResources. Not exist text file.");
                return null;
            }

            List<T> loadAllDataList = new List<T>();
            List<T> eachLoadDataList = null;

            for (int textAssetIndex = 0; textAssetIndex < textAssets.Length; ++textAssetIndex)
            {
                if (textAssets[textAssetIndex] == null)
                    continue;

                eachLoadDataList = DeserializeTextAsset<T>(textAssets[textAssetIndex]);
                if (eachLoadDataList == null)
                    continue;

                loadAllDataList.AddRange(eachLoadDataList);
            }

            return loadAllDataList;
        }

        List<T> DeserializeTextAsset<T>(TextAsset textAsset)
        {
            List<T> loadDataList = null;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));

            using (System.IO.StringReader reader = new System.IO.StringReader(textAsset.text))
            {
                loadDataList = xmlSerializer.Deserialize(reader) as List<T>;
            }

            return loadDataList;
        }

        public List<T> LoadPersistentData<T>(string fileName)
        {
            string dataPath = GetDataPath<T>();
            string loadDataName = Application.persistentDataPath + "/" + dataPath + fileName;
            Debug.LogFormat("Xml load persistent data in path : {0}", loadDataName);

            if (File.Exists(loadDataName) == false)
            {
                Debug.LogWarning("Fail LoadPersistentData. Not exist text file.");
                return null;
            }
                
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));

            StreamReader file = new StreamReader(loadDataName);
            List<T> loadList = xmlSerializer.Deserialize(file) as List<T>;
			file.Close();
			
            return loadList;
		}

        #endregion

        public string GetDataPath<T>()
        {
            Attribute[] attributes = Attribute.GetCustomAttributes(typeof(T));
            if (attributes.Length <= 0)
                return "";

            SerializeDataPathAttribute dataPathAttribute = attributes[0] as SerializeDataPathAttribute;
            if (dataPathAttribute == null)
                return "";

            return dataPathAttribute.DataPath();
        }

        public string GetDataPath(Type type)
        {
            Attribute[] attributes = Attribute.GetCustomAttributes(type);
            if (attributes.Length <= 0)
                return "";

            SerializeDataPathAttribute dataPathAttribute = attributes[0] as SerializeDataPathAttribute;
            if (dataPathAttribute == null)
                return "";

            return dataPathAttribute.DataPath();
        }
	}
}