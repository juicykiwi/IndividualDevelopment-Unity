using UnityEngine;
using System;
using System.Collections;
using CustomXmlSerializerUtil;

[Serializable]
[SerializeDataPath("")]
public class UserInfoData
{
    public int _id = 0;
	public int _lastClearStage = 0;

	public void Reset()
	{
		_id = 1;
		_lastClearStage = 0;
	}
}
