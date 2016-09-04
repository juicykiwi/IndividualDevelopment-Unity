using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CustomXmlSerializerUtil;

[Serializable]
[SerializeDataPath("XmlData/TileData/")]
public class FieldInfoData
{
    public int _id = 0;
	public FieldIndex _fieldIndex = new FieldIndex();

	public List<TileInfoData> _tileInfoList = new List<TileInfoData>();
	public List<ItemInfoData> _itemInfoList = new List<ItemInfoData>();
	public List<PortalInfoData> _portalInfoList = new List<PortalInfoData>();
	public List<HiderInfoData> _hiderInfoList = new List<HiderInfoData>();
}


