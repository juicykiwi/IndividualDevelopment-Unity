using UnityEngine;
using System;
using System.Collections;
using CustomXmlSerializerUtil;

[Serializable]
[SerializeDataPath("Data/TileObjectInfoData/")]
public class TileObjectInfoData
{
	public string _prefabName = "";

	public int _uniqueId = 0;
}