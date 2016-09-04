using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FieldMaker : MonoBehaviour {

	public enum BrushType
	{
		Select,
		Tile,
		Item,
		Potal,
		Hider,
		Eraser,
	}

	public Action _fieldTouchAction = null;

	public BrushType _brushType = BrushType.Tile;

	public GameObject _selectTile = null;
	public GameObject _selectItem = null;
	public GameObject _selectPortal = null;
	public GameObject _selectHider = null;

	public List<GameObject> _selectedObjectList = new List<GameObject>();
	public List<GameObject> _tileList = new List<GameObject>();
	public List<GameObject> _itemList = new List<GameObject>();
	public List<GameObject> _portalList = new List<GameObject>();
	public List<GameObject> _hiderList = new List<GameObject>();

	public GameObject _savePopup = null;
	public GameObject _loadPopup = null;

	public int _tileSliderValue = 0;
	public int _itemSliderValue = 0;
	public int _portalSliderValue = 0;
	public int _hiderSliderValue = 0;
	
	public int _saveLoadFieldIndex = 1;
	
#region FieldMaker base

	// Use this for initialization
	void Start () {
		_tileList = LoadDataManager.instance._tilePrefabDict.GetPrefabList();
		if (_tileList.Count > 0)
		{
			_selectTile = _tileList[0];
		}

		_itemList = LoadDataManager.instance._itemPrefabDict.GetPrefabList();
		_portalList = LoadDataManager.instance._portalPrefabDict.GetPrefabList();
		_hiderList = LoadDataManager.instance._hiderPrefabDict.GetPrefabList();

		HidePopupAll();

		if (TouchManager.instance != null)
		{
			TouchManager.instance._actionTouchBegan += DelTouchBegan;
		}

		if (KeyManager.instance != null)
		{
			KeyManager.instance._keyDirectionInputAction += OnKeyInput;
		}
	}

	void OnDestroy () {
		if (TouchManager.instance != null)
		{
			TouchManager.instance._actionTouchBegan -= DelTouchBegan;
		}

		if (KeyManager.instance != null)
		{
			KeyManager.instance._keyDirectionInputAction -= OnKeyInput;
		}

		if (_fieldTouchAction != null)
		{
			_fieldTouchAction = null;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetSelectType(BrushType type)
	{
		switch(_brushType)
		{
		case BrushType.Select:
		{
			if (type != BrushType.Select)
			{
				_selectedObjectList.Clear();
			}
		}
			break;

		default:
			break;
		}

		_brushType = type;
	}

	public void SaveField(int fieldIndex)
	{
		if (fieldIndex > 0)
		{
			FieldManager.instance.SaveField (fieldIndex, 1);
		}
	}

	public void LoadField(int fieldIndex)
	{
		if (fieldIndex > 0)
		{
			CameraManager.instance.SetPosition(Vector2.zero);
			ClearSelectedObjectList();

			FieldManager.instance.LoadField(fieldIndex);
		}
	}

	public void ClearField()
	{
		CameraManager.instance.SetPosition(Vector2.zero);
		ClearSelectedObjectList();

		FieldManager.instance.ClearField();
	}

	public void ShowSavePopup()
	{
		_savePopup.SetActive(true);
	}

	public void ShowLoadPopup()
	{
		_loadPopup.SetActive(true);
	}

	public void HidePopupAll()
	{
		_savePopup.SetActive(false);
		_loadPopup.SetActive(false);
	}

	void DelTouchBegan(Vector2 touchPos)
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(touchPos, Vector2.zero);

		switch (_brushType)
		{
		case BrushType.Select:
		{
			ClearSelectedObjectList();

			foreach (RaycastHit2D hit in hits)
			{
				SelectedHitObject(hit);
			}
		}
			break;

		case BrushType.Tile:
		{
			foreach (RaycastHit2D hit in hits)
			{
				DestroyHitTile(hit);
			}
			
			CreateTile(touchPos);
		}
			break;

		case BrushType.Item:
		{
			foreach (RaycastHit2D hit in hits)
			{
				DestroyHitItem(hit);
			}

			CreateItem(touchPos);
		}
			break;

		case BrushType.Potal:
		{
			foreach (RaycastHit2D hit in hits)
			{
				DestroyHitPortal(hit);
			}
			
			CreatePortal(touchPos);
		}
			break;

		case BrushType.Hider:
		{
			foreach (RaycastHit2D hit in hits)
			{
				DestroyHitHider(hit);
			}

			CreateHider(touchPos);
		}
			break;

		case BrushType.Eraser:
		{
			foreach (RaycastHit2D hit in hits)
			{
				DestroyHitTile(hit);
				DestroyHitItem(hit);
				DestroyHitPortal(hit);
				DestroyHitHider(hit);
			}
		}
			break;
		}

		if (_fieldTouchAction != null)
		{
			_fieldTouchAction();
		}
	}
	
	void OnKeyInput(Direction direction)
	{
		CameraManager.instance.Move(direction);
	}
	
#endregion


#region Selected

	void ClearSelectedObjectList()
	{
		_selectedObjectList.Clear();
	}
	
	void SelectedHitObject(RaycastHit2D hit)
	{
		if (hit.collider == null)
			return;

		_selectedObjectList.Add(hit.collider.gameObject);
	}

#endregion


#region Tile

	void CreateTile(Vector2 touchPos)
	{
		if (_selectTile == null)
			return;
		
		Vector3 createPos = GameHelper.RoundPos(touchPos);
		
		TileBase tile = _selectTile.GetComponent<TileBase>();
		if (tile == null)
			return;
		
		TileInfoData tileInfo = tile.CreateTileInfoData();
		tileInfo._postion = createPos;
		
		FieldManager.instance.AddTile(_selectTile, tileInfo);
	}

	void DestroyHitTile(RaycastHit2D hit)
	{
		if (hit.collider == null)
			return;

		TileController tileController = hit.collider.GetComponent<TileController>();
		if (tileController == null)
			return;

		if (_selectTile == null)
		{
			tileController.RemoveLastTileEventObejct(tileController.TileList);

			if (tileController.TilesCount <= 0)
			{
				FieldManager.instance.RemoveTileController(tileController);
			}
			return;
		}
	}

#endregion

#region Item

	void CreateItem(Vector2 touchPos)
	{
		if (_selectItem == null)
			return;
		
		Vector3 createPos = GameHelper.RoundPos(touchPos);

		TileController tileController = FieldManager.instance.GetTileControllerWithPos(GameHelper.RoundPos(createPos));
		if (tileController == null)
			return;
		
		Item itemPrefab = _selectItem.GetComponent<Item>();
		if (itemPrefab == null)
			return;
		
		Item newItem = Item.Create(itemPrefab.prefab);
		FieldManager.instance.AddItem(newItem, createPos);
	}

	void DestroyHitItem(RaycastHit2D hit)
	{
		if (hit.collider == null)
			return;
		
		Item item = hit.collider.GetComponent<Item>();
		if (item == null)
			return;
		
		Destroy(item.gameObject);
	}
	
#endregion

#region Portal

	void CreatePortal(Vector2 touchPos)
	{
		if (_selectPortal == null)
			return;
		
		Vector3 createPos = GameHelper.RoundPos(touchPos);

		TileController tileController = FieldManager.instance.GetTileControllerWithPos(GameHelper.RoundPos(createPos));
		if (tileController == null)
			return;
		
		PortalBase portalPrefab = _selectPortal.GetComponent<PortalBase>();
		if (portalPrefab == null)
			return;
		
		PortalBase newPortal = PortalBase.Create(portalPrefab._prefab);
		FieldManager.instance.AddPortal(newPortal, createPos);
	}

	void DestroyHitPortal(RaycastHit2D hit)
	{
		if (hit.collider == null)
			return;
		
		PortalBase portal = hit.collider.GetComponent<PortalBase>();
		if (portal == null)
			return;
		
		Destroy(portal.gameObject);
	}

#endregion
	
#region Hider

	void CreateHider(Vector2 touchPos)
	{
		if (_selectHider == null)
			return;
		
		Vector3 createPos = GameHelper.RoundPos(touchPos);
		
		TileController tileController = FieldManager.instance.GetTileControllerWithPos(GameHelper.RoundPos(createPos));
		if (tileController == null)
			return;
		
		HiderBase hiderPrefab = _selectHider.GetComponent<HiderBase>();
		if (hiderPrefab == null)
			return;
		
		HiderBase newHider = HiderBase.Create(hiderPrefab.prefab);
		FieldManager.instance.AddHider(newHider, createPos);
	}
	
	void DestroyHitHider(RaycastHit2D hit)
	{
		if (hit.collider == null)
			return;
		
		HiderBase hider = hit.collider.GetComponent<HiderBase>();
		if (hider == null)
			return;
		
		Destroy(hider.gameObject);
	}
#endregion

}
