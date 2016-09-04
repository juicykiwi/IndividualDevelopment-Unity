using UnityEngine;
using System.Collections;

public enum PortalType
{
}

public class PortalBase : TileEventObject
{	
	protected int _id = 0;

	public virtual string _prefab { get { return "PortalBase"; } }

	public static PortalBase Create(PortalInfoData info)
	{
		if (info == null)
			return null;

		PortalBase portal = Create(info._prefab);
		if (portal == null)
			return null;

		portal.SetInfoData(info);
		return portal;
	}

	public static PortalBase Create(string prefabName)
	{
		GameObject prefab = LoadDataManager.instance._portalPrefabDict.GetPrefabByName(prefabName);
		if (prefab == null)
			return null;
		
		GameObject newObj = Instantiate(prefab) as GameObject;
		PortalBase portal = newObj.GetComponent<PortalBase>();
		if (portal == null)
		{
			Destroy(newObj);
			return null;
		}
		
		return portal;
	}

	public virtual void SetInfoData(PortalInfoData info)
	{
		_id = info._id;
		transform.position = GameHelper.RoundPos(info._postion);
	}
	
	public virtual PortalInfoData CreateItemInfo()
	{
		PortalInfoData info = new PortalInfoData();
		info._id = _id;
		info._prefab = _prefab;
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
