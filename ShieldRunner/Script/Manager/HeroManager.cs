using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroManager : Singleton<HeroManager>, ITitleLoadAsynk
{
    public const int HeroObjectId = 1;

    HeroObjectFactory _heroFactory = new HeroObjectFactory();
    public HeroObjectFactory HeroFactory { get { return _heroFactory; } }

	// HeroInfoData
	Dictionary<int, BattleObjectInfoData> _heroInfoDataDict =
		new Dictionary<int, BattleObjectInfoData>();

	public int HeroInfoDataDictCount { get { return _heroInfoDataDict.Count; } }

	// Created hero
	Dictionary<int, HeroObject> _createdHeroObjectDict = new Dictionary<int, HeroObject>();

	// Select hero
	HeroObject _selectHeroObejct = null;
	public HeroObject SelectHeroObejct
	{
		get { return _selectHeroObejct; }
		set { _selectHeroObejct = value; }
	}

    // Method

    #region ITitleLoadAsynk

    public void InitAsynk()
    {
        CreateInstance();
        Init(
            BattleObjectDataManager.instance.GetBattleInfoDataList(BattleObjectType.Hero));
    }

    #endregion

	#region Init

	public void Init(List<BattleObjectInfoData> infoDataList)
	{
		_heroInfoDataDict.Clear();

		foreach (BattleObjectInfoData infoData in infoDataList)
		{
			if (infoData == null)
				continue;

			_heroInfoDataDict.Add(infoData._uniqueId, infoData);
		}
	}

	#endregion

	#region HeroInfoData

    public BattleObjectInfoData GetHeroInfoData()
    {
        return GetHeroInfoData(HeroObjectId);
    }
	
	public BattleObjectInfoData GetHeroInfoData(int uniqueId)
	{
		if (_heroInfoDataDict.ContainsKey(uniqueId) == false)
		{
			Debug.Log("Fail get HeroInfoData. Not ContainsKey : " + uniqueId.ToString());
			return null;
		}
		
		return _heroInfoDataDict[uniqueId];
	}
	
	#endregion

	#region Created hero

	public HeroObject GetCreatedHeroObject(int createdId)
	{
		if (_createdHeroObjectDict.ContainsKey(createdId) == false)
		{
			Debug.LogWarning("Fail GetCratedHeroObject. Not contains key : " + createdId.ToString());
			return null;
		}

		return _createdHeroObjectDict[createdId];
	}

	public void AddCreatedHeroInDict(HeroObject heroObject)
	{
		if (heroObject == null)
			return;

		heroObject.transform.SetParent(transform);
		_createdHeroObjectDict.Add(heroObject.CreatedId, heroObject);
	}

	public void RemoveCreatedHeroInDict(int createdId)
	{
		if (_createdHeroObjectDict.ContainsKey(createdId) == false)
			return;

		_createdHeroObjectDict.Remove(createdId);

		if (SelectHeroObejct != null)
		{
			if (SelectHeroObejct.CreatedId == createdId)
			{
				SelectHeroObejct = null;
			}
		}
	}

	public void RemoveCreatedHeroDictAll()
	{
		_createdHeroObjectDict.Clear();
		GameObjectHelper.DestroyChildAll<HeroObject>(transform);

		SelectHeroObejct = null;
	}

	#endregion
}
