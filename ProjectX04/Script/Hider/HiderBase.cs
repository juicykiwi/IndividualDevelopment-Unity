using UnityEngine;
using System.Collections;

public class HiderBase : TileEventObject
{

	protected int _id = 0;
	public virtual string prefab { get { return "HiderBase"; } }

	[Range(0f, 100f)]
	public float _lookThroughRatio = 0f;

	public static HiderBase Create(HiderInfoData info)
	{
		if (info == null)
			return null;
		
		HiderBase hider = Create(info._prefab);
		if (hider == null)
			return null;
		
		hider.SetInfoData(info);
		return hider;
	}
	
	public static HiderBase Create(string prefabName)
	{
		GameObject prefab = LoadDataManager.instance._hiderPrefabDict.GetPrefabByName(prefabName);
		if (prefab == null)
			return null;
		
		GameObject newObj = Instantiate(prefab) as GameObject;
		HiderBase hider = newObj.GetComponent<HiderBase>();
		if (hider == null)
		{
			Destroy(newObj);
			return null;
		}
		
		return hider;
	}

	public virtual void SetInfoData(HiderInfoData info)
	{
		_id = info._id;
		_lookThroughRatio = info._lookThroughRatio;
		transform.position = GameHelper.RoundPos(info._postion);
	}
	
	public virtual HiderInfoData CreateInfoData()
	{
		HiderInfoData info = new HiderInfoData();
		info._prefab = prefab;
		info._lookThroughRatio = _lookThroughRatio;
		info._postion = GameHelper.RoundPos(transform.position);
		
		return info;
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
		return false;
	}

	public override void DestroyObject()
	{
		Destroy(gameObject);
	}
}
