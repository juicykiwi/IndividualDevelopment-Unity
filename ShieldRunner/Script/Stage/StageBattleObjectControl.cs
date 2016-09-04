using UnityEngine;
using System.Collections;

public class StageBattleObjectControl : MonoBehaviour
{
	[SerializeField]
	float _offsetLenght = 5f;
	
	[SerializeField]
	float _lastCreatedBattleObjectPos = 0f;

    // Method

	void OnDestroy()
	{
		StopTileCreator();
	}
	
	public void StartBattleObjectCreator(bool clearLastData = true)
	{
		if (clearLastData == true)
		{
			_lastCreatedBattleObjectPos = 0f;
		}
		
		StartCoroutine(CreatorLoopCoroutine());
	}
	
	public void StopTileCreator()
	{
		StopCoroutine(CreatorLoopCoroutine());
	}
	
	IEnumerator CreatorLoopCoroutine()
	{
		if (_lastCreatedBattleObjectPos == 0f)
		{
			SetLastCreatedTilePosForStart();
		}
		
		while (true)
		{
			CreateBattleObjectLoop();
			
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	void SetLastCreatedTilePosForStart()
	{
		Vector3 screenMinPoint = CameraManager.instance.GetScreenToWorldPoint(Vector2.zero);
		
		_lastCreatedBattleObjectPos = screenMinPoint.x - _offsetLenght;
	}
	
	void CreateBattleObjectLoop()
	{
		while(true)
		{
			if (IsNeedCreateBattleObject() == false)
				break;
			
			BattleObject createdBattleObject = null;

			// need create battle object code by stage data 
			
			if (createdBattleObject == null)
			{
				_lastCreatedBattleObjectPos += 1f;
			}
			else
			{
				_lastCreatedBattleObjectPos += createdBattleObject.transform.localScale.x;
			}
		}
	}
	
	bool IsNeedCreateBattleObject()
	{
		Vector2 screenSize = ScreenManager.instance.ScreenSize;
		Vector3 screenMaxPoint = CameraManager.instance.GetScreenToWorldPoint(screenSize);
		
		return (_lastCreatedBattleObjectPos <= screenMaxPoint.x + _offsetLenght);
	}
	
	public bool IsNeedRemoveBattleObject(BattleObject battleObject)
	{
		if (battleObject == null)
			return false;
		
		Vector3 pos = battleObject.transform.position;
		
		Vector3 screenMinPoint = CameraManager.instance.GetScreenToWorldPoint(Vector2.zero);
		
		return (pos.x <= screenMinPoint.x - _offsetLenght);
	}
}
