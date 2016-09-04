using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CustomXmlSerializerUtil;

public class ChaManager : ManagerBase<ChaManager> {

	public Dictionary<int, ChaController> _chaDict = new Dictionary<int, ChaController>();

	public List<ChaController> _test_chaList = new List<ChaController>();

	string ChaObjectName = "ChaList";
	GameObject _chaObject = null;
	
	int _userChaId = 0;
	List<int> _enemyChaIdList = new List<int>();

	// Method

	protected override void Awake () {
		base.Awake();

		_chaObject = new GameObject(ChaObjectName);
		_chaObject.transform.SetParent(this.transform);
	}

//	public override void ActionSceneLoaded(SceneType sceneType)

	public override void ActionSceneClosed(SceneType sceneType)
	{
		SceneType[] typeArray = new SceneType[] { SceneType.GameStage };

		if (IsHaveSceneTypeFormArray(sceneType, typeArray) == true)
		{
			ClearCha();
		}
	}

	public ChaController CreateCha(int chaId, ChaType chaType)
	{
        ChaModelData chaModel = ChaModelManager.instance.DataById(chaId);
		if (chaModel == null)
			return null;

		GameObject chaPrefab = LoadDataManager.instance._chaPrefabDict.GetPrefabByName(chaModel._chaPrefab);
		if (chaPrefab == null)
			return null;
		
		GameObject newChaObj = Instantiate(chaPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		if (newChaObj == null)
			return null;
		
		ChaController cha = newChaObj.GetComponent<ChaController>() as ChaController;
		if (cha == null)
		{
			Destroy(newChaObj);
			return null;
		}

		cha.Init(chaModel, _chaDict.Count + 1);
		newChaObj.transform.SetParent(_chaObject.transform);

		_chaDict.Add(cha.idInStage, cha);
		_test_chaList.Add(cha);

		switch (chaType)
		{
		case ChaType.User:
		{
			_userChaId = cha.idInStage;
		}
			break;

		case ChaType.Enemy:
		{
			_enemyChaIdList.Add(cha.idInStage);
		}
			break;
		default:
			break;
		}

		return cha;
	}

	public void ClearCha()
	{
		foreach (ChaController cha in _chaDict.Values)
		{
			if (cha == null)
				continue;

			Destroy(cha.gameObject);
		}
		_chaDict.Clear();
		_test_chaList.Clear();

		_userChaId = 0;
		_enemyChaIdList.Clear();
	}

	public ChaController GetUserCha()
	{
		if (_userChaId == 0)
			return null;

		if (_chaDict.ContainsKey(_userChaId) == false)
			return null;

		return _chaDict[_userChaId];
	}

	public List<ChaInfoData> GetChaInfoDataList()
	{
        return ChaDataManager.instance.DataList;
	}

	public ChaInfoData GetChaInfoDataWithId(int id)
	{
        return ChaDataManager.instance.DataById(id);
	}

	public ChaInfoData GetUserChaInfoData()
	{
		return GetChaInfoDataWithId(1);
	}
}
