using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class TileInfoData
{
    public int _id = 0;
	public string _tilePrefab = "";
	public Vector3 _postion = Vector3.zero;
	
	public List<string> _variableList = new List<string>();
}
