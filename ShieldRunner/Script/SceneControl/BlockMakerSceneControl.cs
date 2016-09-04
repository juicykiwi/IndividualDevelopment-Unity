using UnityEngine;
using System.Collections;

#if UNITY_EDITOR

public enum BlockMakerBrushType
{
	Eraser,
	Tile,
	Monster,
    PickupItem,
}

public partial class BlockMakerSceneControl : SceneControl<BlockMakerSceneControl>
{
	[SerializeField]
	BlockObject _makeBlockObject = null;
	public BlockObject MakeBlockObject
	{
		get
		{
			return _makeBlockObject;
		}
		set
		{
			_makeBlockObject = value;

			if (_makeBlockObject != null)
			{
				_makeBlockObject.transform.SetParent(transform);
			}
		}
	}

    protected override SceneControlType SceneControlType
    {
        get { return SceneControlType.BlockMaker; }
    }

    [SerializeField]
    protected IBlockToolBrush _brush = new BlockToolTileBrush();
    public IBlockToolBrush Brush { set { _brush = value; } }

	// Method

	protected override void Start()
	{
        base.Start();

		StartBlockMaker();

		BlockMakerWindow.Init();
	}

	void StartBlockMaker()
	{
		TouchManager.instance._touchClickedEvent += OnTouchClicked;

		// x, y axis line render

		const float lineSize = 100f;

		LineRendererObject xLine = LineRendererObject.Create();
		xLine.SetDefault();
		xLine.SetPosition(Vector3.zero, Vector3.right * lineSize);

		LineRendererObject yLine = LineRendererObject.Create();
		yLine.SetDefault();
		yLine.SetPosition(Vector3.zero, Vector3.up * lineSize);

		// Make object

		MakeBlockObject = BlockObject.CreateEmptyBlockObject();
	}
	
	void Update()
	{
		UpdateKeyDown();
	}

	// Load / Save / Clear

	public void LoadBlockObject(float blockId)
	{
		ClearBlockObject();

		BlockDataManager.instance.LoadDataById(blockId);
		
		BlockInfoData infoData = BlockDataManager.instance.GetLoadedBlockInfoData();
		if (infoData != null)
		{
			MakeBlockObject = BlockObject.CreateBlockObject(infoData._blockId, Vector2.zero);
		}

		if (MakeBlockObject == null)
		{
			MakeBlockObject = BlockObject.CreateEmptyBlockObject();
		}
	}

	public void SaveBlockObject(float blockId)
	{
		MakeBlockObject.RefreshInfoData(blockId);

		BlockDataManager.instance.ClearData();
        BlockDataManager.instance.AddInfoData(MakeBlockObject.InfoData);

		BlockDataManager.instance.SaveDataById(blockId);
	}

	public void ClearBlockObject()
	{
        if (MakeBlockObject != null)
        {
            MakeBlockObject.RemoveBlockObject();
        }

        BattleObject[] battleObjects = FindObjectsOfType<BattleObject>();
        foreach (BattleObject battleObject in battleObjects)
        {
            battleObject.RemoveActionObject();
        }

        MakeBlockObject = BlockObject.CreateEmptyBlockObject();
	}

    // key control

	void UpdateKeyDown()
	{
		float extendSpeed = 5f;

		bool isSlowMove = Input.GetKey(KeyCode.LeftShift);
		if (isSlowMove == true)
		{
			extendSpeed = 1f;
		}

		float moveSpeedX = Input.GetAxis("Horizontal") * Time.deltaTime * extendSpeed;
		float moveSpeedY = Input.GetAxis("Vertical") * Time.deltaTime * extendSpeed;
		
		CameraManager.instance.MainCamera.transform.Translate(new Vector3(moveSpeedX, moveSpeedY, 0f));
	}

    // event

	public void OnTouchClicked(Vector2 touchPos)
	{
		if (MakeBlockObject == null)
		{
			Debug.LogWarning("MakeBlockObject is null.");
			return;
		}

        if (_brush is BlockToolEraserBrush)
        {
            Erase(touchPos);
            return;
        }

        _brush.ElementId = BlockMakerWindow.instance.ElementId;
        IBlockElement blockElement = _brush.Draw(touchPos);
        MakeBlockObject.AddElement(blockElement);
	}

    public void Erase(Vector2 touchPos)
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(touchPos, touchPos);
        Debug.Log("hits count : " + hits.Length.ToString());

        foreach (RaycastHit2D hit in hits)
        {
            TileObject tile = hit.collider.gameObject.GetComponent<TileObject>();
            if (tile != null)
            {
                MakeBlockObject.RemoveTile(tile.CreatedTileId);
                continue;
            }

            ModelControl modelControl = hit.collider.gameObject.GetComponent<ModelControl>();
            if (modelControl != null)
            {
                MonsterObject monster = modelControl.RootActionObject as MonsterObject;
                if (monster != null)
                {
                    MakeBlockObject.RemoveMonster(monster);
                    continue;
                }

                PickupItemObject pickupItem = modelControl.RootActionObject as PickupItemObject;
                if (pickupItem != null)
                {
                    MakeBlockObject.RemovePickupItem(pickupItem);
                }
            }
        }
    }
}

#endif