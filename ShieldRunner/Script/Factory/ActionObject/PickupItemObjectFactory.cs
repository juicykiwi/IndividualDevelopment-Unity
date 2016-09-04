using UnityEngine;
using System.Collections;

public class PickupItemObjectFactory
{
    static int _createdIdLast = 0;
    public static int GetNewCreatedId { get { return ++_createdIdLast; } }

    // Method

    public PickupItemObject CreatePickupItemObject(int uniqueId)
    {
        PickupItemInfoData infoData = PickupItemDataManager.instance.InfoDataAtUniqueId(uniqueId);
        if (infoData == null)
            return null;

        GameObject prefab = PickupItemDataManager.instance.PrefabDataAtName(infoData._prefabName);
        if (prefab == null)
            return null;

        GameObject newObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        if (newObject == null)
            return null;

        PickupItemObject pickupItemObject = newObject.GetComponent<PickupItemObject>();
        if (pickupItemObject == null)
        {
            GameObject.Destroy(newObject);
            return null;
        }

        pickupItemObject.InfoData = infoData;
        pickupItemObject.CreatedId = GetNewCreatedId;

        return pickupItemObject;
    }
}
