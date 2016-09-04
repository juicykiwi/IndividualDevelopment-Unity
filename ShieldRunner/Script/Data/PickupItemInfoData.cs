using UnityEngine;
using System;
using System.Collections;
using CustomXmlSerializerUtil;

[Serializable]
[SerializeDataPath("Data/PickupItemInfoData/")]
public class PickupItemInfoData
{
    public string _prefabName = "";

    public int _uniqueId = 0;

    public PickupItemType _type = PickupItemType.None;
    public int _value = 0;
}