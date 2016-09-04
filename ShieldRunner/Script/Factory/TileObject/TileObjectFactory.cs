using UnityEngine;
using System.Collections;

public class TileObjectFactory
{
    static int _createdTileIdLast = 0;
    public static int GetNewCreatedTileId { get { return ++_createdTileIdLast; } }

    // Method

    public TileObject CreateTileObject(int uniqueId)
    {
        TileObjectInfoData infoData = TileObjectDataManager.instance.InfoDataAtUniqueId(uniqueId);
        if (infoData == null)
        {
            Debug.LogError("Fail! CreateTileObject(). infoData is null.");
            return null;
        }

        GameObject prefab = TileObjectDataManager.instance.PrefabDataAtName(infoData._prefabName);
        if (prefab == null)
        {
            Debug.LogError("Fail! CreateTileObject(). Not finded prefab.");
            return null;
        }

        GameObject newObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        if (newObject == null)
        {
            Debug.LogError("Fail! CreateTileObject(). newObject is null.");
            return null;
        }

        TileObject tileObject = newObject.GetComponent<TileObject>();
        if (tileObject == null)
        {
            Debug.LogError("Fail! CreateTileObject(). tileObject is null.");
            GameObject.Destroy(newObject);
            return null; 
        }

        // set info data
        tileObject.InfoData = infoData;
        tileObject.CreatedTileId = GetNewCreatedTileId;

        return tileObject;
    }
}
