using UnityEngine;
using System;
using System.Collections;
using CustomXmlSerializerUtil;

[Serializable]
[SerializeDataPath("XmlData/")]
public class ChaModelData
{
    public int _id = 0;
	public string _chaPrefab = "";
}
