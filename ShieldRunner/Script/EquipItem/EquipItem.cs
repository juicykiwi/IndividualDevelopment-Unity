using UnityEngine;
using System.Collections;

public class EquipItem : MonoBehaviour
{
    [SerializeField]
    EquipItemInfoData _infoData = null;
    public EquipItemInfoData InfoData
    { 
        get { return _infoData; } 
        set { _infoData = value; }
    }

    [SerializeField]
    SpriteRenderer _modelSprite = null;
    public SpriteRenderer ModelSprite { get { return _modelSprite; } }

    [SerializeField]
    float _scaleAfterEquip = 1f;
    public float ScaleAfterEquip { get { return _scaleAfterEquip; } }

    // Method

    #region 생성

    public static EquipItem CreateEquipItem(int uniqueId)
    {
        EquipItemInfoData findData = EquipItemDataManager.instance.EquipItemAtUniqueId(uniqueId);
        EquipItem equipItem = CreateEquipItem(findData);
        return equipItem;
    }

    static EquipItem CreateEquipItem(EquipItemInfoData infoData)
    {
        if (infoData == null)
            return null;

        GameObject prefab = EquipItemDataManager.instance.EquipItemPrefabAtName(infoData._prefabName);
        if (prefab == null)
            return null;

        GameObject newObject = Instantiate(prefab) as GameObject;
        EquipItem equipItem = newObject.GetComponent<EquipItem>();
        equipItem.InfoData = infoData.Clone();
        
        return equipItem;
    }

    #endregion

    public void RemoveItem()
    {
        Destroy(gameObject);
    }
        
    #region 내구도 컨트롤

    public void ReduceDurability()
    {
        if (SceneHelper.IsStageScene() == true)
        {
            if (StageSceneControl.instance.TestMode == true)
            {
                return;
            }
        }

        if (_infoData._durabilityValue <= 0)
            return; 

        _infoData._durabilityValue -= 1;
    }

    public bool IsBroken()
    {
        return (_infoData._durabilityValue <= 0);
    }

    #endregion
}
