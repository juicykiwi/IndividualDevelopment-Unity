using UnityEngine;
using System;
using System.Collections;
using CustomXmlSerializerUtil;

[Serializable]
[SerializeDataPath("XmlData/")]
public class StageInfoData
{
    public int _id = 0;
	public int _stageLevel = 0;
	public int _fieldIndex = 0;
	public int _turnMax = 0;
}
