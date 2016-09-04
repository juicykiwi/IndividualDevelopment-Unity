using UnityEngine;
using System;
using System.Collections;
using CustomXmlSerializerUtil;

[Serializable]
[SerializeDataPath("XmlData/")]
public class ChaInfoData
{
    public int _id = 0;
	public int _chaPrefabId = 0;

	public Team _team = Team.None;

	public int _life = 0;
	public int _detectRange = 0;
	
	public float _moveSpeed = 0.0f;
	public float _attackSpeed = 0.0f;
	public float _idleWaitTime = 0.0f;
}
