using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileController : MonoBehaviour{

	public enum TileActionType
	{
		Enter,
		EnterComplete,
		Leave,
	}

	protected int _tileId = 0;
	public int tileId { set { _tileId = value; } }

	public bool _isUnderCha = false;

	List<TileBase> _tiles = new List<TileBase>();
	public int TilesCount { get { return _tiles.Count; } }
	public List<TileBase> TileList { get { return _tiles; } }

	List<Item> _items = new List<Item>();
	public int ItemCount { get { return _items.Count; } }
	public List<Item> ItemList { get { return _items; } }

	List<PortalBase> _portalList = new List<PortalBase>();
	public int PortalCount { get { return _portalList.Count; } }
	public List<PortalBase> PortalList { get { return _portalList; } }

	List<HiderBase> _hiderList = new List<HiderBase>();
	public int HiderCount { get { return _hiderList.Count; } }
	public List<HiderBase> HiderList { get { return _hiderList; } }

	// Method

	public static TileController CreateTileController()
	{
		GameObject newObj = new GameObject(typeof(TileController).Name);
		newObj.AddComponent<BoxCollider2D>();
		return newObj.AddComponent<TileController>();
	}

	public void DestroyTileController()
	{
		RemoveTileEventObjectList(TileList);
		RemoveTileEventObjectList(ItemList);
		RemoveTileEventObjectList(PortalList);
		RemoveTileEventObjectList(HiderList);

		Destroy(gameObject);
	}

	public Vector2 GetPos()
	{
		return GameHelper.RoundPos(transform.position);
	}

	public bool IsEnableMoveTile()
	{
		if (_tiles.Count <= 0)
			return false;

		foreach (TileBase tile in _tiles)
		{
			if (tile.IsEnableMoveTile() == false)
				return false;
		}

		return true;
	}

	public int GetTilePathWeight()
	{
		int weight = 0;
		foreach (TileBase tile in _tiles)
		{
			if (tile == null)
				continue;

			weight = Mathf.Max(weight, tile._tilePathWeight);
		}

		return weight;
	}
	
	public void DoTileActionForEnter(ChaController cha)
	{
		_isUnderCha = true;

		List<TileEventObject> tileEventObjectList = new List<TileEventObject>();

		tileEventObjectList.AddRange(_items.ToArray());
		UseTileEventObject(tileEventObjectList, cha, TileActionType.Enter);
		tileEventObjectList.Clear();

		tileEventObjectList.AddRange(_portalList.ToArray());
		UseTileEventObject(tileEventObjectList, cha, TileActionType.Enter);
		tileEventObjectList.Clear();
	}

	public void DoTileActionForEnterComplete(ChaController cha)
	{
		List<TileEventObject> tileEventObjectList = new List<TileEventObject>();

		tileEventObjectList.AddRange(_items.ToArray());
		UseTileEventObject(tileEventObjectList, cha, TileActionType.EnterComplete);
		tileEventObjectList.Clear();
		
		tileEventObjectList.AddRange(_portalList.ToArray());
		UseTileEventObject(tileEventObjectList, cha, TileActionType.EnterComplete);
		tileEventObjectList.Clear();
	}

	public void DoTileActionForLeave(ChaController cha)
	{
		_isUnderCha = false;

		List<TileEventObject> tileEventObjectList = new List<TileEventObject>();
		
		tileEventObjectList.AddRange(_items.ToArray());
		UseTileEventObject(tileEventObjectList, cha, TileActionType.Leave);
		tileEventObjectList.Clear();
		
		tileEventObjectList.AddRange(_portalList.ToArray());
		UseTileEventObject(tileEventObjectList, cha, TileActionType.Leave);
		tileEventObjectList.Clear();
	}

	#region TileEventObejct

	void UseTileEventObject<T>(List<T> tList, ChaController cha, TileActionType actionType) where T : TileEventObject
	{
		if (tList.Count <= 0)
			return;

		List<T> runActionObjectList = new List<T>();
		runActionObjectList.AddRange(tList);

		foreach (T each in runActionObjectList)
		{
			if (each == null)
				continue;

			switch (actionType)
			{
			case TileActionType.Enter:
				each.EnterTile(cha);
				break;

			case TileActionType.EnterComplete:
				each.EnterCompleteTile(cha);
				break;

			case TileActionType.Leave:
				each.LeaveTile(cha);
				break;

			default:
				break;
			}

			if (each.IsReserveDestroy() == true)
			{
				tList.Remove(each);
				each.DestroyObject();
			}
		}
	}

	public void AddTileEventObject<T>(List<T> tList, T tileEventObject) where T : TileEventObject
	{
		if (tList == null)
			return;

		if (tileEventObject == null)
			return;

		float zPos = _tiles.Count * -1f;

		tList.Add (tileEventObject);
		tileEventObject.transform.SetParent(this.transform);
		tileEventObject.transform.localPosition = new Vector3(0f, 0f, zPos);
	}

	public void RemoveLastTileEventObejct<T>(List<T> tList) where T : TileEventObject
	{
		if (tList == null)
			return;

		if (tList.Count <= 0)
			return;

		T tileEventObejct = tList[tList.Count - 1];
		tList.Remove(tileEventObejct);
		tileEventObejct.DestroyObject();
	}

	public void RemoveTileEventObject<T>(List<T> tList, T t) where T : TileEventObject
	{
		if (tList == null)
			return;

		if (t == null)
			return;

		tList.Remove(t);
		t.DestroyObject();
	}

	public void RemoveTileEventObjectList<T>(List<T> tList) where T : TileEventObject
	{
		if (tList == null)
			return;

		foreach (T each in tList)
		{
			if (each == null)
				continue;

			each.DestroyObject();
		}

		tList.Clear();
	}

	#endregion
}
