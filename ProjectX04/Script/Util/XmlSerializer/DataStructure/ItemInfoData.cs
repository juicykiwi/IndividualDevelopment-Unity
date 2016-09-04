using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ItemInfoData
{
    public int _id = 0;
	public int _itemIndex = 0;
	public string _prefab = "";

	public Vector3 _postion = Vector3.zero;

	public List<string> _variableList = new List<string>();
}
