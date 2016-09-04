using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager : Singleton<MonsterManager>, ITitleLoadAsynk
{
    MonsterObjectFactory _monsterFactory = new MonsterObjectFactory();
    public MonsterObjectFactory MonsterFactory { get { return _monsterFactory; } }

    GameObjectPool<int, MonsterObject> _monsterPool = new GameObjectPool<int, MonsterObject>();
    public GameObjectPool<int, MonsterObject> MonsterPool { get { return _monsterPool; } }

    // MonsterInfoData
    Dictionary<int, BattleObjectInfoData> _monsterInfoDataDict =
        new Dictionary<int, BattleObjectInfoData>();

    public int MonsterInfoDataDictCount { get { return _monsterInfoDataDict.Count; } }

    // Method

    #region ITitleLoadAsynk

    public void InitAsynk()
    {
        CreateInstance();

        BattleObjectDataManager instance = BattleObjectDataManager.instance;
        Init_Monster(instance.GetBattleInfoDataList(BattleObjectType.Monster));
    }

    #endregion

    public void PreSettingPool(int monsterId, int count)
    {
        MonsterObjectFactory factory = new MonsterObjectFactory();

        for (int index = 0; index < count; ++index)
        {
            MonsterObject monsterObject = factory.CreateMonsterObject(monsterId, true);
            if (monsterObject == null)
            {
                Debug.LogError("Fail! MonsterManager.PreSettingPool(). monsterObject is null.");
                break;
            }
            
            _monsterPool.Enqueue(monsterObject.InfoData._uniqueId, monsterObject);
        }
    }

    public void Clear()
    {
        GameObjectHelper.DestroyChildAll<MonsterObject>(transform);
        _monsterPool.Clear();
    }

    public MonsterObject CreateMonster(int uniqueId)
    {
        MonsterObject monsterObject = null;

        bool IsInGame = SceneHelper.IsInGame();
        if (IsInGame == true)
        {
            monsterObject = _monsterPool.Dequeue(uniqueId);
        }

        if (monsterObject == null)
        {
            monsterObject = _monsterFactory.CreateMonsterObject(uniqueId, IsInGame);
        }

        return monsterObject;
    }

	#region MonsterInfoData

	public void Init_Monster(List<BattleObjectInfoData> infoDataList)
	{
		_monsterInfoDataDict.Clear();

		foreach (BattleObjectInfoData infoData in infoDataList)
		{
			if (infoData == null)
				continue;
			
			_monsterInfoDataDict.Add(infoData._uniqueId, infoData);
		}
	}

	public BattleObjectInfoData GetMonsterInfoData(int uniqueId)
	{
		if (_monsterInfoDataDict.ContainsKey(uniqueId) == false)
		{
			Debug.Log("Fail get monsterInfoData. Not ContainsKey : " + uniqueId.ToString());
			return null;
		}

		return _monsterInfoDataDict[uniqueId];
	}

	#endregion
}
