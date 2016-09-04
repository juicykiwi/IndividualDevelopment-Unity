using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PortalInfoData
{
    public int _id = 0;
	public int _portalId = 0;
	public string _prefab = "";
	
	public Vector3 _postion = Vector3.zero;
	
	public List<string> _variableList = new List<string>();
}
