using UnityEngine;
using System.Collections;

public class BattleObjectEquipItemSlot : MonoBehaviour
{
    [SerializeField]
    EquipItem _equipItem = null;
    public EquipItem EquipItem { get { return _equipItem; } }

    // Method

    #region 아이템 장착 / 해제

    // 방패 기본 장착 처리 (스테이지 시작 시)
    public void EquipShield(int baseShieldId, int lastEquipShieldId)
    {
        bool useBaseShield = false;

        if (lastEquipShieldId == 0 || lastEquipShieldId == baseShieldId)
            useBaseShield = true;

        EquipItem lastEqupItemShield = EquipItem.CreateEquipItem(lastEquipShieldId);
        if (lastEqupItemShield == null)
            useBaseShield = true;

        if (useBaseShield == true)
        {
            EquipItem baseEqupItemShield = EquipItem.CreateEquipItem(baseShieldId);
            if (baseEqupItemShield == null)
                return;

            Equip(baseEqupItemShield);
            PlayerDataManager.instance.PlayerData._lastEquipedShieldId = baseShieldId;
        }
        else
        {
            Equip(lastEqupItemShield);
        }
    }

    void Equip(EquipItem equipItem)
    {
        _equipItem = equipItem;
        if (_equipItem == null)
            return;

        _equipItem.transform.SetParent(transform);
        _equipItem.transform.localPosition = Vector3.zero;
        _equipItem.transform.localRotation = Quaternion.identity;

        SpriteRenderer parentSpriteRenderer = transform.parent.transform.GetComponent<SpriteRenderer>();
        if (parentSpriteRenderer != null)
        {
            _equipItem.ModelSprite.sortingOrder = parentSpriteRenderer.sortingOrder + 1;

            Vector3 equipItemScale = new Vector3(_equipItem.ScaleAfterEquip, _equipItem.ScaleAfterEquip, _equipItem.ScaleAfterEquip);
            _equipItem.transform.localScale = equipItemScale;
        }
    }

    public void Unequip()
    {
        if (_equipItem == null)
            return;

        _equipItem.RemoveItem();
        _equipItem = null;
    }

    #endregion

    #region 내구도 처리

    public int GetEquipItemDurability()
    {
        if (_equipItem == null)
            return 0;

        return _equipItem.InfoData._durabilityValue;
    }

    public void IncreaseEquipItemDurability(int increaseValue)
    {
        if (_equipItem == null)
            return;

        _equipItem.InfoData._durabilityValue += increaseValue;
    }

    #endregion
}
