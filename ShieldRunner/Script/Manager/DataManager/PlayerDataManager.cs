using UnityEngine;
using System.Collections;
using CustomXmlSerializerUtil;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerDataManager 
    : DataManager<PlayerInfoData, PlayerDataManager>, ITitleLoadAsynk
{
    const string PlayerDataSerializeEx01Name = "Ex01";

    public PlayerInfoData PlayerData
    { 
        get 
        { 
            if (_dataList.Count <= 0)
            {
                SaveFirstPlayerData();
            }

            return _dataList[0];
        } 
    }

    [SerializeField]
    int _gameTryCount = 0;
    public int GameTryCount
    {
        get { return _gameTryCount; }
        set { _gameTryCount = value; }
    }

    // Method
        
    #region ITitleLoadAsynk

    public void InitAsynk()
    {
        CreateInstance();
        Init();
    }

    #endregion

    void SaveFirstPlayerData()
    {
        _dataList.Add(new PlayerInfoData());
        SaveData();
    }

    #region DataManager<>

	public override void LoadData()
	{
        LoadData("PlayerInfoData" + PlayerDataSerializeEx01Name, DataLoadType.Persistent);

        if (_dataList.Count <= 0)
        {
            LoadOldData();

            if (_dataList.Count <= 0)
            {
                SaveFirstPlayerData();
            }
            else
            {
                SaveData();
            }
        }
	}

	public override void SaveData()
	{
        SaveData("PlayerInfoData" + PlayerDataSerializeEx01Name, DataLoadType.Persistent);
	}

	#endregion

	#region 최고 이동 기록

	public float BestMoveDistance()
	{
        return PlayerData._bestMoveDistance;
	}

	public void SaveNewBestMoveDistance(float distance)
	{
        PlayerData._bestMoveDistance = distance;
        SaveData();
	}

	#endregion

    #region Gold

    public int HaveGold()
    {
        return PlayerData._haveGold;
    }

    public void IncreaseGold(int increaseValue, bool isSave = false)
    {
        PlayerData._haveGold += increaseValue;

        if (isSave == true)
        {
            SaveData();
        }
    }

    public void DecreaseGold(int increaseValue, bool isSave = false)
    {
        PlayerData._haveGold -= increaseValue;
        if (PlayerData._haveGold < 0)
            PlayerData._haveGold = 0;

        if (isSave == true)
        {
            SaveData();
        }
    }

    #endregion

    #region 방패 착용 정보

    public int LastEquipShieldId()
    {
        int lastEquipedShieldId = PlayerData._lastEquipedShieldId;
        if (lastEquipedShieldId <= 0)
        {
            lastEquipedShieldId = HeroManager.instance.GetHeroInfoData()._baseEquipShieldId;
        }

        return lastEquipedShieldId;
    }

    public void SetLastEquipShieldId(int newEquipShieldId)
    {
        PlayerData._lastEquipedShieldId = newEquipShieldId;
        SaveData();
    }

    #endregion

    #region 방패 인챈트 카운트

    public int GetEnchantCount()
    {
        return Mathf.Max(0, PlayerData._enchatCount);
    }

    public int NextEnchantCost()
    {
        return (PlayerData._enchatCount + 1) * 100;
    }

    public void Enchant()
    {
        PlayerData._enchatCount += 1;
        SaveData();
    }

    #endregion
}

#if UNITY_EDITOR

[CustomEditor(typeof(PlayerDataManager))]
public class PlayerDataManagerEditor
    : DataManagerEditor<PlayerInfoData, PlayerDataManager>
{
}

#endif