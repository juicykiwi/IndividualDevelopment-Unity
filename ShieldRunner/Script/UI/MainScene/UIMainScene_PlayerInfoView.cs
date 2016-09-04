using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMainScene_PlayerInfoView : MonoBehaviour
{
    [SerializeField]
    Text _shieldText = null;

    [SerializeField]
    Text _goldText = null;

    [SerializeField]
    Text _bestRunText = null;

    // Method

    public void UpdateInfoView()
    {
        int gold = PlayerDataManager.instance.HaveGold();
        float distance = PlayerDataManager.instance.BestMoveDistance();

        int shieldDurability = 0;
        int lastEquipShieldId = PlayerDataManager.instance.LastEquipShieldId();

        EquipItemInfoData equipItemInfoData = EquipItemDataManager.instance.EquipItemAtUniqueId(lastEquipShieldId);
        if (equipItemInfoData != null)
        {
            shieldDurability = equipItemInfoData._durabilityValue;
            shieldDurability += PlayerDataManager.instance.GetEnchantCount();
        }

        _goldText.text = gold.ToString();
        _shieldText.text = shieldDurability.ToString();
        _bestRunText.text = distance.ToString("F0") + " m";
    }
}
