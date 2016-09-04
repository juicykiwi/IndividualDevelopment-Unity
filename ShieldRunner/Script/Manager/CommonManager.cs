using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public interface ITitleLoadAsynk
{
    void InitAsynk();
}

public class CommonManager : Singleton<CommonManager>
{
	[SerializeField]
	bool _isLoaded = false;
	public bool IsLoaded { get { return _isLoaded; } }

    GameObject _dataManagerObject = null;

    // Method

	public void Init()
	{
		if (_isLoaded == true)
			return;

        _dataManagerObject = new GameObject("DataManager");
        _dataManagerObject.transform.SetParent(transform);

		/* Set Hierarchy */

        Application.targetFrameRate = 60;
        Debug.Log("Application.targetFrameRate : " + Application.targetFrameRate.ToString());

		ScreenManager.instance.transform.SetParent(transform);

        CameraManager.instance.Init();
		CameraManager.instance.transform.SetParent(transform);

		TouchManager.instance.transform.SetParent(transform);

		SerializeManager.instance.transform.SetParent(transform);

		BlockManager.instance.transform.SetParent(transform);

        TileManager.instance.transform.SetParent(transform);

        PickupItemManager.instance.transform.SetParent(transform);
		
		// DataManager init

        BlockDataManager.instance.LoadDataAll();
        BlockDataManager.instance.transform.SetParent(_dataManagerObject.transform);

        StageDataManager.instance.LoadDataAll();
        StageDataManager.instance.transform.SetParent(_dataManagerObject.transform);

		PlayerDataManager.instance.Init();
        PlayerDataManager.instance.transform.SetParent(_dataManagerObject.transform);

		EquipItemDataManager.instance.Init();
        EquipItemDataManager.instance.transform.SetParent(_dataManagerObject.transform);

        UpgradeDataManager.instance.Init();
        UpgradeDataManager.instance.transform.SetParent(_dataManagerObject.transform);

		TileObjectDataManager.instance.Init();
        TileObjectDataManager.instance.transform.SetParent(_dataManagerObject.transform);

        PickupItemDataManager.instance.Init();
        PickupItemDataManager.instance.transform.SetParent(_dataManagerObject.transform);
		
		// BattleObject init

        BattleObjectDataManager.instance.Init();
        BattleObjectDataManager.instance.transform.SetParent(_dataManagerObject.transform);
		
		HeroManager.instance.Init(
			BattleObjectDataManager.instance.GetBattleInfoDataList(BattleObjectType.Hero));
        HeroManager.instance.transform.SetParent(transform);
		
		MonsterManager.instance.Init_Monster(
			BattleObjectDataManager.instance.GetBattleInfoDataList(BattleObjectType.Monster));
        MonsterManager.instance.transform.SetParent(transform);

		_isLoaded = true;
	}

    public void InitAsynk(Action<int> loadingAction, Action completeAction)
    {
        if (_isLoaded == true)
            return;

        _isLoaded = true;

        _dataManagerObject = new GameObject("DataManager");
        _dataManagerObject.transform.SetParent(transform);

        List<Type> loadTypeList = new List<Type>();

        loadTypeList.Add(typeof(ScreenManager));
        loadTypeList.Add(typeof(CameraManager));
        loadTypeList.Add(typeof(TouchManager));
        loadTypeList.Add(typeof(SerializeManager));
        loadTypeList.Add(typeof(BlockManager));
        loadTypeList.Add(typeof(TileManager));
        loadTypeList.Add(typeof(PickupItemManager));

        loadTypeList.Add(typeof(BlockDataManager));
        loadTypeList.Add(typeof(StageDataManager));
        loadTypeList.Add(typeof(PlayerDataManager));
        loadTypeList.Add(typeof(EquipItemDataManager));
        loadTypeList.Add(typeof(UpgradeDataManager));
        loadTypeList.Add(typeof(TileObjectDataManager));
        loadTypeList.Add(typeof(PickupItemDataManager));

        loadTypeList.Add(typeof(BattleObjectDataManager));
        loadTypeList.Add(typeof(HeroManager));
        loadTypeList.Add(typeof(MonsterManager));

        StartCoroutine(LoadDataCoroutine(loadTypeList, loadingAction, completeAction));
    }

    IEnumerator LoadDataCoroutine(List<Type> loadTypeList, Action<int> loadingAction, Action completeAction)
    {
        if (loadTypeList.Count <= 0)
        {
            if (completeAction != null)
            {
                completeAction();
            }
            yield break;
        }

        for (int index = 0; index < loadTypeList.Count; ++index)
        {
            if (loadTypeList[index] == null)
                continue;

            GameObject newGameObject = new GameObject(loadTypeList[index].Name, loadTypeList[index]);
            if (newGameObject == null)
                continue;

            Component component = newGameObject.GetComponent(loadTypeList[index]);
            if (component == null)
                continue;

            ITitleLoadAsynk titleLoadAsynk = component as ITitleLoadAsynk;
            if (titleLoadAsynk != null)
            {
                titleLoadAsynk.InitAsynk();
            }
                
            component.transform.SetParent(transform);

            int percent = (int)((float)index / loadTypeList.Count * 100f);
            if (loadingAction != null)
            {
                loadingAction(percent);
            }

            yield return new WaitForEndOfFrame();
        }

        if (completeAction != null)
        {
            completeAction();
        }
        yield break;
    }
}
