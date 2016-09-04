using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CheckRayHitType
{
	None,
	UserCha,
	EnemyCha,
	Tile,
}

public enum StageState
{
	None,
	Init,
	Play,
	Stop,
	End,
}

public class StageManager : ManagerBase<StageManager> {

	StageState _stageState = StageState.None; 

	ChaController _tochedCha = null;

	public int _startStageIndex = 1;

	bool _isEnableChaMoveInClick = false;

	// Method

	protected override void Awake () {
		base.Awake();
	}

	#region Load/Close event

	public override void ActionSceneLoaded(SceneType sceneType)
	{
		SceneType[] typeArray = new SceneType[] {
			SceneType.SelectStage,
			SceneType.GameStage
		};
		
		if (IsHaveSceneTypeFormArray(sceneType, typeArray) == true)
		{
			if (TouchManager.instance != null)
			{
				TouchManager.instance._actioTouchClicked += this.OnTouchClicked;
				TouchManager.instance._actionTouchMoveEnded += this.TouchMoveEnded;
			}
			
			if (EventManager.instance != null)
			{
				EventManager.instance._actionSuccessMission += this.SuccessStageClear;
				EventManager.instance._actionFailMission += this.FailStageClear;
			}

			if (KeyManager.instance != null)
			{
				KeyManager.instance._keyDirectionInputAction += KeyDownArrow;
			}

		}
	}
	
	public override void ActionSceneClosed(SceneType sceneType)
	{
		SceneType[] typeArray = new SceneType[] {
			SceneType.SelectStage,
			SceneType.GameStage
		};
		
		if (IsHaveSceneTypeFormArray(sceneType, typeArray) == true)
		{
			if (TouchManager.instance != null)
			{
				TouchManager.instance._actioTouchClicked -= this.OnTouchClicked;
				TouchManager.instance._actionTouchMoveEnded -= this.TouchMoveEnded;
			}
			
			if (EventManager.instance != null)
			{
				EventManager.instance._actionSuccessMission -= this.SuccessStageClear;
				EventManager.instance._actionFailMission -= this.FailStageClear;
			}

			if (KeyManager.instance != null)
			{
				KeyManager.instance._keyDirectionInputAction -= KeyDownArrow;
			}
		}
	}

	#endregion

	#region Start setting

	public void StartStage(int stageIndex)
	{
		_stageState = StageState.Init;

		ClearStage();

		// Stage info

		StageInfoData stageInfo = GetStageInfoData(stageIndex);
		if (stageInfo == null)
			return;

		// Field.
		
		FieldManager.instance.LoadField(stageInfo._fieldIndex);
		FieldManager.instance.InitTilePath();

		CameraManager.instance.SetEnableMoveRect(FieldManager.instance._fieldRect);
		CameraManager.instance.MoveForMoveRectCenter();

		StartStageCha();

		UserInfoManager.instance.SetCoin(0);

		_stageState = StageState.Play;

		CameraManager.instance.MoveStartForTarget(
			ChaManager.instance.GetUserCha().gameObject, Vector2.zero, Vector2.zero);
	}

	public void WarpField(int fieldIndex, Vector2 warpPos)
	{
		_stageState = StageState.Init;
		
		ClearStage();

		FieldManager.instance.LoadField(fieldIndex);
		CameraManager.instance.SetEnableMoveRect(FieldManager.instance._fieldRect);
		CameraManager.instance.MoveForMoveRectCenter();

		StartStageCha(warpPos);

		_stageState = StageState.Play;

		CameraManager.instance.MoveStartForTarget(
			ChaManager.instance.GetUserCha().gameObject, Vector2.zero, Vector2.zero);
	}

	public void StartStageCha()
	{
		Vector2 spawnPos = FieldManager.instance.GetGamePortalSpawnUserPos();
		StartStageCha(spawnPos);
	}

	public void StartStageCha(Vector2 userSpawnPos)
	{
		// User Cha Spawn
		
		UserChaSpawn(userSpawnPos);

		// Enemy Cha Spawn
		
		List<PortalSpawnEnemyCha> portalSpawnEnemyList = FieldManager.instance.GetGamePortalSpawnEnemyList();
		
		foreach (PortalSpawnEnemyCha portalSpawn in portalSpawnEnemyList)
		{
			if (portalSpawn == null)
				continue;
			
			EnemyChaSpawn(GameHelper.RoundPos(portalSpawn.transform.position), portalSpawn._spawnEnemyId);
		}
	}

	ChaController UserChaSpawn(Vector2 pos)
	{
		ChaInfoData chaInfo = ChaManager.instance.GetUserChaInfoData();
		if (chaInfo == null)
			return null;
		
		return ChaSpawn(chaInfo, pos, ChaType.User);
	}
	
	ChaController EnemyChaSpawn(Vector2 pos, int chaId)
	{
		ChaInfoData chaInfo = ChaManager.instance.GetChaInfoDataWithId(chaId);
		if (chaInfo == null)
			return null;
		
		return ChaSpawn(chaInfo, pos, ChaType.Enemy);
	}
	
	ChaController ChaSpawn(ChaInfoData chaInfo, Vector2 pos, ChaType chaType)
	{
		if (chaInfo == null)
			return null;
		
		ChaController cha = ChaManager.instance.CreateCha(chaInfo._chaPrefabId, chaType);
		if (cha == null)
			return null;
		
		cha.stat._life = chaInfo._life;
		cha.stat._moveSpeed = chaInfo._moveSpeed;
		cha.stat._attackSpeed = chaInfo._attackSpeed;
		cha.stat._idleWaitTime = chaInfo._idleWaitTime;
		cha.stat._detectRange = chaInfo._detectRange;
		
		TileController tileController = FieldManager.instance.GetTileControllerWithPos(pos);
		if (tileController == null)
			return null;
		
		cha.MoveForce(tileController.transform.position);
		cha.gameObject.SetActive(true);
		
		return cha;
	}

	#endregion

	#region Clear setting

	void ClearStage()
	{
		CameraManager.instance.MoveEndForTarget();
		EffectManager.instance.ClearEffect();

		FieldManager.instance.ClearField();
		ChaManager.instance.ClearCha();
	}

	#endregion

	public StageInfoData GetStageInfoData(int stageLevel)
	{
        List<StageInfoData> stageInfoList = StageDataManager.instance.DataList;
		if (stageInfoList.Count <= 0)
			return null;
		
		StageInfoData stageInfo = stageInfoList.Find(
			(StageInfoData stageInfoInList) => { return stageInfoInList._stageLevel == stageLevel; } );

		return stageInfo;
	}

	public void SuccessStageClear()
	{
		if (_stageState == StageState.End)
			return;

		_stageState = StageState.End;
		UserInfoManager.instance.ClearStage(_startStageIndex);

		if (TouchManager.instance != null)
		{
			TouchManager.instance._actioTouchClicked -= this.OnTouchClicked;
			TouchManager.instance._actionTouchMoveEnded -= this.TouchMoveEnded;
		}

		instance.Invoke("GoSceneForSelectStage", 1.0f);
	}

	public void FailStageClear()
	{
		if (_stageState == StageState.End)
			return;

		_stageState = StageState.End;

		if (TouchManager.instance != null)
		{
			TouchManager.instance._actioTouchClicked -= this.OnTouchClicked;
			TouchManager.instance._actionTouchMoveEnded -= this.TouchMoveEnded;
		}

		instance.Invoke("GoSceneForSelectStage", 2.0f);
	}

	public void GoSceneForSelectStage()
	{
		ClearStage();

		SceneGameStage.instance.StartSelectStageScene();
	}

//	IEnumerator StartStageCoroutine(int stageLevel)
//	{
//		yield return new WaitForSeconds(2.0f);
//
//		StartStage(stageLevel);
//		yield return null;
//	}

	public bool IsStartStage(int stageLevel)
	{
		if (GetStageInfoData(stageLevel) == null)
			return false;

		return true;
	}

	void OnTouchClicked(Vector2 touchPos)
	{
		List<ChaController> userChaList = new List<ChaController>();
		List<ChaController> otherChaList = new List<ChaController>();
		List<TileController> tileControllerList = new List<TileController>();

		bool isTouchHit = CheckPosRayHit(touchPos, ref userChaList, ref otherChaList, ref tileControllerList);

		if (isTouchHit == false)
		{
			_tochedCha = null;

			if (EventManager.instance._actionCancelTouchUserCha != null)
			{
				EventManager.instance._actionCancelTouchUserCha();
			}

			return;
		}

		if (OnTouchUserCha(userChaList) == true)
		{
			_tochedCha = userChaList[0];

			if (EventManager.instance._actionTouchUserCha != null)
			{
				EventManager.instance._actionTouchUserCha();
			}

			return;
		}

		if (EventManager.instance._actionCancelTouchUserCha != null)
		{
			EventManager.instance._actionCancelTouchUserCha();
		}

		if (OnTouchOtherCha(otherChaList) == true)
			return;

		if (OnTouchTile(tileControllerList) == false)
			return;

		// User cha move.

		if (_tochedCha && _isEnableChaMoveInClick == true)
		{
			_tochedCha.aiController.RequestMove(tileControllerList[0].transform.position);
			_tochedCha = null;
		}
	}

	public void TouchMoveEnded(Vector2 originPos, Vector2 endPos, float distance)
	{
		if (EventManager.instance._actionCancelTouchUserCha != null)
		{
			EventManager.instance._actionCancelTouchUserCha();
		}

		ChaController user = ChaManager.instance.GetUserCha();
		if (user == null)
			return;

		Direction direction = TouchManager.instance.GetTouchDirection(originPos, endPos);
		if (direction == Direction.None)
			return;

		// User cha move.

		user.aiController.RequestMove(direction);
		_tochedCha = null;
	}

	public void KeyDownArrow(Direction direction)
	{
		ChaController user = ChaManager.instance.GetUserCha();
		if (user == null)
			return;

		user.aiController.RequestMove(direction);
	}

	public bool CheckPosRayHit(Vector2 pos,
	                           ref List<ChaController> userChaList,
	                           ref List<ChaController> otherChaList,
	                           ref List<TileController> tileControllerList)
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.zero);

		if (hits == null || hits.Length <= 0)
			return false;

		foreach (RaycastHit2D hit in hits)
		{
			if (hit.collider == null)
				continue;

			{
				ChaController cha = hit.collider.GetComponent<ChaUser>();
				if (cha)
				{
					userChaList.Add(cha);
				}
			}

			{
				ChaEnemy cha = hit.collider.GetComponent<ChaEnemy>();
				if (cha)
				{
					otherChaList.Add(cha);
				}
			}

			{
				TileController tileController = hit.collider.GetComponent<TileController>();
				if (tileController)
				{
					tileControllerList.Add(tileController);
				}
			}
		}

		if (userChaList.Count > 0 || otherChaList.Count > 0 || tileControllerList.Count > 0)
			return true;

		return false;
	}

	public bool CheckPosRayHit(Vector2 checkPos, CheckRayHitType checkRayHitType)
	{
		List<ChaController> userChaList = new List<ChaController>();
		List<ChaController> otherChaList = new List<ChaController>();
		List<TileController> tileControllerList = new List<TileController>();

		bool isHit = CheckPosRayHit(checkPos, ref userChaList, ref otherChaList, ref tileControllerList);
		if (isHit == false)
			return false;

		bool isHitForType = false;

		switch (checkRayHitType)
		{
		case CheckRayHitType.UserCha:
		{
			isHitForType = userChaList.Count > 0;
		}
			break;

		case CheckRayHitType.EnemyCha:
		{
			isHitForType = otherChaList.Count > 0;
		}
			break;

		case CheckRayHitType.Tile:
		{
			isHitForType = tileControllerList.Count > 0;
		}
			break;

		default:
			break;
		}

		return isHitForType;
	}

	bool OnTouchUserCha(List<ChaController> userChaList)
	{
		if (userChaList.Count <= 0)
			return false;

		return true;
	}

	bool OnTouchOtherCha(List<ChaController> otherChaList)
	{
		if (otherChaList.Count <= 0)
			return false;

		return true;
	}

	bool OnTouchTile(List<TileController> tileControllerList)
	{
		if (tileControllerList == null || tileControllerList.Count <= 0)
			return false;

		return true;
	}
}
