using UnityEngine;
using System.Collections;

public partial class StageMakerSceneControl : SceneControl<StageMakerSceneControl>
{
	[SerializeField]
	StageObject _makeStageObject = null;
	public StageObject MakeStageObject
	{
		get
		{
			return _makeStageObject;
		}
		set
		{
			_makeStageObject = value;
			
			if (_makeStageObject != null)
			{
				_makeStageObject.transform.SetParent(transform);
			}
		}
	}

	[SerializeField]
	int _selectBlockId = 0;
	public int SelectBlockId { set { _selectBlockId = value; } }

    protected override SceneControlType SceneControlType
    {
        get { return SceneControlType.BlockMaker; }
    }

    // Method

	protected override void Start()
	{
        base.Start();

		BlockDataManager.instance.LoadDataAll();

		StartStageMaker();
	}

	void StartStageMaker()
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

		/* Make object */

		MakeStageObject = StageObject.CreateEmptyStageObject();
	}

	void Update()
	{
		UpdateKeyDown();
	}

	// Load / Save / Clear

	public void LoadStage(int stageId)
	{
		ClearStage();

		StageDataManager.instance.LoadDataById(stageId);

        StageInfoData infoData = StageDataManager.instance.SelectedStageData;
        if (infoData == null)
        {
            Debug.LogError("Error! LoadStage(). StageInfoData is null.");
            return;
        }

		MakeStageObject = StageObject.CreateStageObject(infoData._stageId);
	}

	public void SaveStage(int stageId)
	{
		StageDataManager.instance.ClearData();
		StageDataManager.instance.Add(MakeStageObject.InfoData);

		StageDataManager.instance.SaveDataById(stageId);
	}

	public void ClearStage()
	{
		if (MakeStageObject == null)
			return;
		
		MakeStageObject.RemoveStageObject();
		MakeStageObject = null;
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
		BlockObject.CreateBlockObject(_selectBlockId, touchPos);
	}
}