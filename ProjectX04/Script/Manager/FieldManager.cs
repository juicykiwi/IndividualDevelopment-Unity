using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CustomXmlSerializerUtil;

public class FieldManager : ManagerBase<FieldManager> {

	#region FieldBase
	
	Dictionary<Vector2, TileController> _posKeyTileControllerDict = new Dictionary<Vector2, TileController>();

	string TileObjectName = "TileList";
	GameObject _tileObject = null;

	public Rect _fieldRect = new Rect();

	// Method

	protected override void Awake () {
		base.Awake();

		_tileObject = new GameObject(TileObjectName);
		_tileObject.transform.SetParent(this.transform);
	}

	void FixedUpdate () {

		// Check not finded field.
//		FieldTest();
	}

	public override void ActionSceneLoaded(SceneType sceneType)
	{
		SceneType[] typeArray = new SceneType[] { SceneType.FieldMaker, SceneType.GameStage };

		if (IsHaveSceneTypeFormArray(sceneType, typeArray) == true)
		{
			EventManager.instance._actionChaMoveStart += this.OnChaTileEnter;
			EventManager.instance._actionChaMoveFinish += this.OnChaTileEnterComplete;
		}
	}
	
	public override void ActionSceneClosed(SceneType sceneType)
	{
		SceneType[] typeArray = new SceneType[] { SceneType.FieldMaker, SceneType.GameStage };

		if (IsHaveSceneTypeFormArray(sceneType, typeArray) == true)
		{
			EventManager.instance._actionChaMoveStart -= this.OnChaTileEnter;
			EventManager.instance._actionChaMoveFinish -= this.OnChaTileEnterComplete;
		}
	}

	void ExpandFieldRect(Vector3 tilepos)
	{
		_fieldRect.xMin = Mathf.Min(_fieldRect.xMin, tilepos.x);
		_fieldRect.xMax = Mathf.Max(_fieldRect.xMax, tilepos.x);
		_fieldRect.yMin = Mathf.Min(_fieldRect.yMin, tilepos.y);
		_fieldRect.yMax = Mathf.Max(_fieldRect.yMax, tilepos.y);
	}

	public void ClearField()
	{
		ClearTile();
	}

	public void SaveField(int fieldMainId, int fieldSubId)
	{
		FieldInfoData fieldInfoData = new FieldInfoData();
		fieldInfoData._id = fieldMainId;
		fieldInfoData._fieldIndex._fieldMainId = fieldMainId;
		fieldInfoData._fieldIndex._fieldSubId = fieldSubId;

		// TileInfoDataList

		List<TileInfoData> tileInfoDataList = GetTileInfoInField();
		fieldInfoData._tileInfoList = tileInfoDataList;

		// ItemInfoDataList

		List<ItemInfoData> itemInfoDataList = GetItemInfoInField();
		fieldInfoData._itemInfoList = itemInfoDataList;

		// PortalInfoDataList
			
		List<PortalInfoData> portalInfoDataList = GetPortalInfoInField();
		fieldInfoData._portalInfoList = portalInfoDataList;

		// HiderInfoDataList

		List<HiderInfoData> hiderInfoDataList = GetHiderInfoInField();
		fieldInfoData._hiderInfoList = hiderInfoDataList;

		// Save FieldInfoData

		System.Type loadObjType = System.Type.GetType("FieldInfoData");

		List<object> objList = new List<object>();
		objList.Add(fieldInfoData);
		
		string postfix = string.Format("{0}", fieldMainId.ToString("D3"));
		CustomXmlSerializerOld.instance.SaveData(objList, loadObjType, postfix);
	}

	public void LoadField(int fieldIndex)
	{
		ClearField();

        FieldInfoData fieldInfoData = FieldDataManager.instance.DataById(fieldIndex);
		if (fieldInfoData == null)
		{
			Debug.LogWarning("Failed LoadField(). Not have fieldIndex");
			return;
		}

		SettingTile(fieldInfoData._tileInfoList);
		SettingPortal(fieldInfoData._portalInfoList);
		SettingItem(fieldInfoData._itemInfoList);
		SettingHider(fieldInfoData._hiderInfoList);
	}

	public void InitTilePath()
	{
		PathFinder.instance.ClearPath();

		foreach (TileController tileController in _posKeyTileControllerDict.Values)
		{
			if (tileController == null)
				continue;

			if (tileController.IsEnableMoveTile() == false)
				continue;

			PathFinder.instance.AddPath(tileController.GetPos(), tileController.GetTilePathWeight());
		}
	}

#endregion

#region Tile related

	public void SettingTile(List<TileInfoData> infoDataList)
	{
		foreach (TileInfoData tileInfo in infoDataList)
		{
			if (tileInfo == null)
				continue;

			GameObject tilePrefab = LoadDataManager.instance._tilePrefabDict.GetPrefabByName(tileInfo._tilePrefab);
			if (tilePrefab == null)
				continue;
			
			AddTile(tilePrefab, tileInfo);
		}
	}

	public void AddTile(GameObject newTilePrefab, TileInfoData tileInfo)
	{
		if (newTilePrefab == null)
			return;
		
		GameObject newTileObj = Instantiate(newTilePrefab, GameHelper.RoundPos(tileInfo._postion), Quaternion.identity) as GameObject;
		if (newTileObj == null)
			return;
		
		TileBase newTile = newTileObj.GetComponent<TileBase>();
		if (newTile == null)
			return;
		
		newTile.InitWithInfoData(tileInfo);
		
		TileController tileController = AddTileController(newTile.GetPos());
		if (tileController == null)
			return;

		tileController.AddTileEventObject<TileBase>(tileController.TileList, newTile);
		
		ExpandFieldRect(newTileObj.transform.position);
	}

	public TileController AddTileController(Vector3 position)
	{
		Vector2 roundPosition = GameHelper.RoundPos(position);

		if (_posKeyTileControllerDict.ContainsKey(roundPosition) == false)
		{
			TileController newTileController = TileController.CreateTileController();
			newTileController.transform.SetParent(_tileObject.transform);
			newTileController.transform.position = roundPosition;
			_posKeyTileControllerDict.Add(roundPosition, newTileController);
		}

		return _posKeyTileControllerDict[roundPosition];
	}

	public TileController GetTileControllerWithPos(Vector2 pos)
	{
		if (_posKeyTileControllerDict.ContainsKey(pos) == false)
			return null;
		
		return _posKeyTileControllerDict[pos];
	}

	public void RemoveTileController(TileController tileController)
	{
		if (tileController == null)
			return;

		_posKeyTileControllerDict.Remove(tileController.GetPos());
		tileController.DestroyTileController();
	}

	public void ClearTile()
	{
		TileController[] tileControllerList = _tileObject.GetComponentsInChildren<TileController>();
		if (tileControllerList.Length > 0)
		{
			foreach (TileController tileController in tileControllerList)
			{
				if (tileController == null)
					continue;
				
				Destroy(tileController.gameObject);
			}
		}
		
		_posKeyTileControllerDict.Clear();
		
		_fieldRect = new Rect();
	}

	List<TileInfoData> GetTileInfoInField()
	{
		List<TileInfoData> tileInfoList = new List<TileInfoData>();
		
		List<TileBase> tileList = GetTileInField();
		for (int index = 0; index < tileList.Count; ++index)
		{
			TileBase tile = tileList[index];
			if (tile == null)
				continue;
			
			TileInfoData tileInfo = tile.CreateTileInfoData();
			tileInfo._id = index;
			
			tileInfoList.Add(tileInfo);
		}
		
		return tileInfoList;
	}
	
	List<TileBase> GetTileInField()
	{
		List<TileBase> tileList = new List<TileBase>();

		foreach (TileController tileController in _posKeyTileControllerDict.Values)
		{
			if (tileController == null)
				continue;

			if (tileController.TilesCount <= 0)
				continue;

			tileList.AddRange(tileController.TileList);
		}

		return tileList;
	}

	public TileController IsMoveTile(TileController tileController)
	{
		if (tileController == null)
			return null;
		
		if (tileController.IsEnableMoveTile() == false)
			return null;
		
		return tileController;
	}

	public void OnChaTileEnter(ChaController cha, Vector2 originPos, Vector2 destPos)
	{
		TileController originTileController = FieldManager.instance.GetTileControllerWithPos(originPos);
		TileEventForLeave(cha, originTileController);
		
		TileController destTileController = FieldManager.instance.GetTileControllerWithPos(destPos);
		TileEventForEnter(cha, destTileController);
	}
	
	public void OnChaTileEnterComplete(ChaController cha, Vector2 originPos, Vector2 destPos)
	{
		TileController destTileController = FieldManager.instance.GetTileControllerWithPos(destPos);
		TileEventForEnterComplete(cha, destTileController);
	}
	
	public void TileEventForEnter(ChaController cha, TileController tileController)
	{
		if (cha == null)
			return;
		
		if (tileController == null)
			return;
		
		tileController.DoTileActionForEnter(cha);
	}

	public void TileEventForEnterComplete(ChaController cha, TileController tileController)
	{
		if (cha == null)
			return;
		
		if (tileController == null)
			return;
		
		tileController.DoTileActionForEnterComplete(cha);
	}
	
	public void TileEventForLeave(ChaController cha, TileController tileController)
	{
		if (cha == null)
			return;
		
		if (tileController == null)
			return;
		
		tileController.DoTileActionForLeave(cha);
	}

#endregion

#region Item Related

	void SettingItem(List<ItemInfoData> infoDataList)
	{
		// Heejun loves Soonah heart BByoung BByoung
		
		foreach (ItemInfoData infoData in infoDataList)
		{
			if (infoData == null)
				continue;
			
			Item item = Item.Create(infoData);
			if (item == null)
				continue;
			
			FieldManager.instance.AddItem(item, item.transform.position);
		}
	}

	public void AddItem(Item newItem, Vector2 pos)
	{
		if (_posKeyTileControllerDict.ContainsKey (pos) == false) {
			Debug.LogWarning ("FieldManager.AddItem : Not exist tile for item");
			return;
		}

		TileController tileController = _posKeyTileControllerDict[pos];
		if (tileController == null)
			return;

		tileController.AddTileEventObject(tileController.ItemList, newItem);
	}

	List<ItemInfoData> GetItemInfoInField()
	{
		List<ItemInfoData> itemInfoList = new List<ItemInfoData>();
		
		List<Item> itemList = GetItemInField();
		for (int index = 0; index < itemList.Count; ++index)
		{
			Item item = itemList[index];
			if (item == null)
				continue;
			
			ItemInfoData itemInfo = item.CreateItemInfo();
			itemInfo._id = index;
			
			itemInfoList.Add(itemInfo);
		}
		
		return itemInfoList;
	}
	
	List<Item> GetItemInField()
	{
		Item[] items = FindObjectsOfType<Item>();
		
		List<Item> itemList = new List<Item> ();
		itemList.AddRange(items);
		return itemList;
	}

#endregion

#region Portal Related

	List<PortalSpawnUserCha> _portalSpawnUserList = new List<PortalSpawnUserCha>();
	List<PortalSpawnEnemyCha> _portalSpawnEnemyList = new List<PortalSpawnEnemyCha>();

	public Vector2 GetGamePortalSpawnUserPos()
	{
		if (_portalSpawnUserList.Count <= 0)
			return Vector2.zero;

		return GameHelper.RoundPos(_portalSpawnUserList[0].transform.position);
	}

	public List<PortalSpawnEnemyCha> GetGamePortalSpawnEnemyList()
	{
		return _portalSpawnEnemyList;
	}

	void SettingPortal(List<PortalInfoData> infoDataList)
	{
		_portalSpawnUserList.Clear();
		_portalSpawnEnemyList.Clear();

		foreach (PortalInfoData infoData in infoDataList)
		{
			if (infoData == null)
				continue;
			
			PortalBase portal = PortalBase.Create(infoData);
			if (portal == null)
				continue;
			
			FieldManager.instance.AddPortal(portal, portal.transform.position);
		}
	}
	
	public void AddPortal(PortalBase newPortal, Vector2 pos)
	{
		if (_posKeyTileControllerDict.ContainsKey (pos) == false) {
			Debug.LogWarning ("FieldManager.AddItem : Not exist tile for item");
			return;
		}
		
		TileController tileController = _posKeyTileControllerDict[pos];
		if (tileController == null)
			return;

		tileController.AddTileEventObject(tileController.PortalList, newPortal);

		PortalSpawnUserCha portalSpawnUserCha = newPortal as PortalSpawnUserCha;
		if (portalSpawnUserCha != null)
		{
			_portalSpawnUserList.Add(portalSpawnUserCha);
		}

		PortalSpawnEnemyCha portalSpawnEnemyCha = newPortal as PortalSpawnEnemyCha;
		if (portalSpawnEnemyCha != null)
		{
			_portalSpawnEnemyList.Add(portalSpawnEnemyCha);
		}
	}

	List<PortalInfoData> GetPortalInfoInField()
	{
		List<PortalInfoData> portalInfoList = new List<PortalInfoData>();
		
		List<PortalBase> portalList = GetPortalInField();
		for (int index = 0; index < portalList.Count; ++index)
		{
			PortalBase portal = portalList[index];
			if (portal == null)
				continue;
			
			PortalInfoData portalInfo = portal.CreateItemInfo();
			portalInfo._id = index;
			
			portalInfoList.Add(portalInfo);
		}
		
		return portalInfoList;
	}
	
	List<PortalBase> GetPortalInField()
	{
		PortalBase[] findedPotalList = FindObjectsOfType<PortalBase>();
		
		List<PortalBase> portalList = new List<PortalBase> ();
		portalList.AddRange(findedPotalList);
		return portalList;
	}

	#endregion

	#region Hider Related

	void SettingHider(List<HiderInfoData> infoDataList)
	{
		foreach (HiderInfoData infoData in infoDataList)
		{
			if (infoData == null)
				continue;
			
			HiderBase hider = HiderBase.Create(infoData);
			if (hider == null)
				continue;
			
			FieldManager.instance.AddHider(hider, hider.transform.position);
		}
	}
	
	public void AddHider(HiderBase newHider, Vector2 pos)
	{
		if (_posKeyTileControllerDict.ContainsKey(pos) == false) {
			Debug.LogWarning ("FieldManager.AddItem : Not exist tile for item");
			return;
		}
		
		TileController tileController = _posKeyTileControllerDict[pos];
		if (tileController == null)
			return;
		
		tileController.AddTileEventObject(tileController.HiderList, newHider);
	}

	List<HiderInfoData> GetHiderInfoInField()
	{
		List<HiderInfoData> hiderInfoList = new List<HiderInfoData>();
		
		List<HiderBase> hiderList = GetHiderInField();
		for (int index = 0; index < hiderList.Count; ++index)
		{
			HiderBase hider = hiderList[index];
			if (hider == null)
				continue;
			
			HiderInfoData hiderInfo = hider.CreateInfoData();
			hiderInfo._id = index;
			
			hiderInfoList.Add(hiderInfo);
		}
		
		return hiderInfoList;
	}
	
	List<HiderBase> GetHiderInField()
	{
		HiderBase[] findedHiderList = FindObjectsOfType<HiderBase>();
		
		List<HiderBase> hiderList = new List<HiderBase> ();
		hiderList.AddRange(findedHiderList);
		return hiderList;
	}

	#endregion
}