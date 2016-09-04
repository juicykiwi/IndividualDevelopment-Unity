using UnityEngine;
using System.Collections;

public class UserInfoManager : ManagerBase<UserInfoManager> {

	public UserInfoData _userInfo = null;

	public int _coin = 0;
	public int coin { get { return _coin; } }

	// Method

	protected override void Awake () {
		base.Awake ();

		LoadUserInfoData();
	}

//	public override void ActionSceneLoaded(SceneType sceneType)
//	public override void ActionSceneClosed(SceneType sceneType)

	public void LoadUserInfoData()
	{
        UserInfoData loadUserInfo = UserDataManager.instance.DataById(1);
		if (loadUserInfo == null)
		{
			if (_userInfo == null)
			{
				_userInfo = new UserInfoData();
				_userInfo.Reset();
			}

			SaveUserInfo();
			return;
		}

		_userInfo = loadUserInfo;
	}

	public void SaveUserInfo()
	{
        UserDataManager.instance.SetUserInfo(_userInfo);
        UserDataManager.instance.SaveData();
	}

	public UserInfoData GetUserInfoData()
	{
		if (_userInfo == null)
		{
			LoadUserInfoData();
		}

		return _userInfo;
	}

	public void ClearStage(int stage)
	{
		if (stage <= _userInfo._lastClearStage)
			return;

		_userInfo._lastClearStage = stage;
		SaveUserInfo();
	}

	public void ResetClearStageInfo()
	{
		_userInfo._lastClearStage = 0;

        UserDataManager.instance.SetUserInfo(_userInfo);
        UserDataManager.instance.SaveData();
	}

	public void AddCoin(int addedCoinCount)
	{
		SetCoin(_coin + addedCoinCount);;
	}

	public void SetCoin(int coinCount)
	{
		_coin = coinCount;

		if (UIManager.instance._changedCoinCountAction != null)
		{
			UIManager.instance._changedCoinCountAction(_coin);
		}
	}
}
