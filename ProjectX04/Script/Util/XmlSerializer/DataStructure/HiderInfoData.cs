using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CustomXmlSerializerUtil;

[Serializable]
[SerializeDataPath("XmlData/")]
public class HiderInfoData
{
    public int _id = 0;
	public string _prefab = "";
	public Vector3 _postion = Vector3.zero;
	public float _lookThroughRatio = 0f;
	public List<string> _variableList = new List<string>();
}
