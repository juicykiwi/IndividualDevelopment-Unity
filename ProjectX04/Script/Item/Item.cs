using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Item : TileEventObject
{
	protected int _itemIndex = 0;

	// Item prefab name
	public virtual string prefab { get { return "Item"; } }

	public int _count = 0;

	// Item name text
	public string _name = "";
	// Item describe text
	public string _describe = "";

	protected bool _isReserveDestroy = false;

	// Method

	public static Item Create(ItemInfoData info)
	{
		if (info == null)
			return null;

		Item item = Create(info._prefab);
		if (item == null)
			return null;
		
		item.SetInfoData(info);
		return item;
	}
	
	public static Item Create(string prefabName)
	{
		GameObject prefab = LoadDataManager.instance._itemPrefabDict.GetPrefabByName(prefabName);
		if (prefab == null)
			return null;
		
		GameObject newObj = Instantiate(prefab) as GameObject;
		Item item = newObj.GetComponent<Item>();
		if (item == null)
		{
			Destroy(newObj);
			return null;
		}
		
		return item;
	}

	public virtual void SetInfoData(ItemInfoData itemInfo)
	{
		_itemIndex = itemInfo._itemIndex;
		transform.position = GameHelper.RoundPos(itemInfo._postion);
	}

	public virtual ItemInfoData CreateItemInfo()
	{
		ItemInfoData tileInfo = new ItemInfoData();
		tileInfo._itemIndex = _itemIndex;
		tileInfo._prefab = prefab;
		tileInfo._postion = GameHelper.RoundPos(transform.position);
		
		return tileInfo;
	}

	public override void EnterTile(ChaController cha)
	{
	}

	public override void EnterCompleteTile(ChaController cha)
	{
	}

	public override void LeaveTile(ChaController cha)
	{
	}

	public override bool IsReserveDestroy()
	{
		return _isReserveDestroy;
	}

	public override void DestroyObject()
	{
		Destroy(gameObject);
	}
}
