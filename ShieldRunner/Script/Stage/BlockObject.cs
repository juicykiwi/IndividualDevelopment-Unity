using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class BlockObject : MonoBehaviour
{
    const float AsyncActionTime = 0.05f;

    float _periodUpdateTime = 0.05f;
    float _currentPeriodUpdateTime = 0f;

	[SerializeField]
	BlockInfoData _infoData = null;
	public BlockInfoData InfoData
	{
		get { return _infoData; }
		set { _infoData = value; }
	}

    // Block bound rect

    [SerializeField]
    Rect _boundRect = new Rect();
    public Rect BountRect { get { return _boundRect; } }

    [SerializeField]
    BoxCollider2D _boxCollider = null;

    // Setting async mode

    [SerializeField]
    bool _isAlreadyStartSettingAsync = false;
    bool _isAlreadyStartRemoveAsync = false;

    public Action<float> CompleteSettingAsyncEvent = null;

    // TileObject
    [SerializeField]
    BlockTileControl _tileControl = new BlockTileControl();
    public BlockTileControl TileControl { get { return _tileControl; } }

    // MonsterObject
    [SerializeField]
    BlockMonsterControl _monsterControl = new BlockMonsterControl();
    public BlockMonsterControl MonsterControl { get { return _monsterControl; } }

    // PickupItemObject
    [SerializeField]
    BlockPickupItemControl _pickupItemControl = new BlockPickupItemControl();
    public BlockPickupItemControl PickupItemControl { get { return _pickupItemControl; } }

    // Method

    void Awake()
    {
        _tileControl.Init(this);
        _monsterControl.Init(this);
        _pickupItemControl.Init(this);
    }

    #region Create, Remove

	public static BlockObject CreateEmptyBlockObject()
	{
		BlockInfoData infoData = new BlockInfoData();
		return CreateBlockObject(infoData, Vector2.zero);
	}

    public static BlockObject CreateBlockObject(float blockId, Vector2 pos, bool notSetting = false)
	{
        BlockInfoData infoData = BlockDataManager.instance.InfoDataAtBlockId(blockId);
        return CreateBlockObject(infoData, pos, notSetting);
	}

    public static BlockObject CreateBlockObject(BlockInfoData infoData, Vector2 pos, bool notSetting = false)
	{
		if (infoData == null)
		{
			Debug.LogError("Fail create BlockObject. infoData is null.");
			return null;
		}

		BlockObject blockObject = GameObjectHelper.NewGameObject<BlockObject>();
		if (blockObject == null)
			return null;

		blockObject.transform.position = GameObjectHelper.RoundPosition2D(pos);
		blockObject.InfoData = infoData;

        if (notSetting == false)
        {
            blockObject.Setting();
        }

		BlockManager.instance.AddBlockObject(blockObject);

		return blockObject;
	}

    // remove

    public void RemoveBlockObject()
    {
        BlockManager.instance.RemoveBlockObject(this);
        Destroy(gameObject);
    }

    public void RefreshInfoData(float blockId)
    {
        _infoData.Clear();

        _infoData._blockId = blockId;

        _tileControl.Sort();
        _infoData.AddTileObjectInfo(_tileControl.ObjectList);

        _monsterControl.Sort();
        _infoData.AddMonsterObjectInfo(_monsterControl.ObjectList);

        _pickupItemControl.Sort();
        _infoData.AddPickupItemObjectInfo(_pickupItemControl.ObjectList);
    }

    #endregion

    #region MonoBehaviour event

    void Update()
    {
        _currentPeriodUpdateTime += Time.deltaTime;
        if (_currentPeriodUpdateTime < _periodUpdateTime)
            return;

        _currentPeriodUpdateTime = 0f;

        float checkPointX = transform.position.x + _boundRect.width;
        float checkPointY = transform.position.y;
        if (CameraManager.instance.IsNeedDestroyByObjectPos(new Vector2(checkPointX, checkPointY)) == false)
            return;

        RemoveBlockObjectAsync();
    }

    #endregion

    #region setting

    public void Setting()
    {
        SettingTile();
        SettingMonster();
        SettingPickupItem();
    }

    public void StartSettingAsyncType()
    {
        if (_isAlreadyStartSettingAsync == true)
            return;

        _isAlreadyStartSettingAsync = true;
        StartCoroutine(SettingAsyncCoroutine());
    }

    IEnumerator SettingAsyncCoroutine()
    {
        yield return StartCoroutine(SettingAsyncTileCoroutine());
        yield return StartCoroutine(SettingAsyncMonsterCoroutine());
        yield return StartCoroutine(SettingAsyncPickupItemCoroutine());

        if (CompleteSettingAsyncEvent != null)
        {
            CompleteSettingAsyncEvent(BountRect.width);
        }
    }

    #endregion

    #region Remove

    public void RemoveBlockObjectAsync()
    {
        if (_isAlreadyStartRemoveAsync == true)
            return;

        _isAlreadyStartRemoveAsync = true;
        StartCoroutine(RemoveAsyncCoroutine());
    }

    IEnumerator RemoveAsyncCoroutine()
    {
        var enumerator = _tileControl.ObjectList.GetEnumerator();

        while (enumerator.MoveNext() == true)
        {
            if (enumerator.Current == null)
                continue;

            enumerator.Current.Remove();
            yield return null;
        }

        RemoveBlockObject();
    }

    #endregion

    #region Block bound rect

    public void UpdateBountRect(Transform trans)
    {
        Rect tileRect = GameObjectHelper.RectByTransform(trans);

        _boundRect.xMin = Mathf.Min(_boundRect.xMin, tileRect.xMin);
        _boundRect.yMin = Mathf.Min(_boundRect.yMin, tileRect.yMin);

        _boundRect.xMax = Mathf.Max(_boundRect.xMax, tileRect.xMax);
        _boundRect.yMax = Mathf.Max(_boundRect.yMax, tileRect.yMax);

        // Check run tool
        if (SceneHelper.IsStageScene() == true)
            return;

        if (_boxCollider == null)
        {
            _boxCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        _boxCollider.offset = _boundRect.center;
        _boxCollider.size = _boundRect.size;
    }

    #endregion

    #region IBlockElement

    public void AddElement(IBlockElement element)
    {
        if (element == null)
            return;

        switch (element.ElementType)
        {
            case BlockElementType.Tile:
                _tileControl.Add(element as TileObject);
                break;

            case BlockElementType.Monster:
                _monsterControl.Add(element as MonsterObject);
                break;

            case BlockElementType.PickupItem:
                _pickupItemControl.Add(element as PickupItemObject);
                break;

            default:
                break;
        }
    }

    #endregion

    #region TileObject

	void SettingTile()
	{
        _tileControl.CreateEnumerator = _infoData._creationTileInfoList.GetEnumerator();
        _tileControl.Create(false);
	}

    IEnumerator SettingAsyncTileCoroutine()
    {
        _tileControl.CreateEnumerator = _infoData._creationTileInfoList.GetEnumerator();
        while (_tileControl.IsEnded == false)
        {
            _tileControl.Create(true);
            yield return null;
        }
    }

    public void RemoveTile(int createdId)
    {
        _tileControl.Remove(createdId);
    }

    #endregion

    #region MonsterObject

	void SettingMonster()
	{
        _monsterControl.CreateEnumerator = _infoData._creationBattleObjectInfoList.GetEnumerator();
        _monsterControl.Create(false);
	}

    IEnumerator SettingAsyncMonsterCoroutine()
    {
        _monsterControl.CreateEnumerator = _infoData._creationBattleObjectInfoList.GetEnumerator();
        while (_monsterControl.IsEnded == false)
        {
            _monsterControl.Create(true);
            yield return null;
        }
    }

    public void RemoveMonster(MonsterObject monsterObject)
    {
        _monsterControl.Remove(monsterObject);
    }

    #endregion

    #region PickupItemObject

    public void SettingPickupItem()
    {
        _pickupItemControl.CreateEnumerator = _infoData._creationPickupItemObjectInfoList.GetEnumerator();
        _pickupItemControl.Create(false);
    }
        
    IEnumerator SettingAsyncPickupItemCoroutine()
    {
        _pickupItemControl.CreateEnumerator = _infoData._creationPickupItemObjectInfoList.GetEnumerator();
        while (_pickupItemControl.IsEnded == false)
        {
            _pickupItemControl.Create(transform);
            yield return null;
        }
    }

    public void RemovePickupItem(PickupItemObject pickupItem)
    {
        _pickupItemControl.Remove(pickupItem);
    }

    #endregion
}
