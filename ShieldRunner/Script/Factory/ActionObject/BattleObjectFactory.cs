using UnityEngine;
using System.Collections;

public abstract class BattleObjectFactory
{
    public abstract BattleObject CreateBattleObject(int uniqueId);

    // Method

    public T NewBattleObject<T>(int uniqueId) where T : BattleObject
    {
        BattleObjectInfoData infoData = BattleObjectDataManager.instance.InfoDataAtUniqueId(uniqueId);
        if (infoData == null)
            return null;

        GameObject prefab = BattleObjectDataManager.instance.PrefabDataAtName(infoData._prefabName);
        if (prefab == null)
            return null;

        GameObject newObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        if (newObject == null)
            return null;

        T battleObject = newObject.GetComponent<T>();
        if (battleObject == null)
        {
            GameObject.Destroy(newObject);
            return null;
        }

        battleObject.InfoData = infoData;
        battleObject.InitStat();

        return battleObject;
    }
}
