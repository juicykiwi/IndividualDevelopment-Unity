using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIStageScene_PlayerInfoView : MonoBehaviour
{
    [SerializeField]
    Text _shieldText = null;

    [SerializeField]
    Text _goldText = null;

    [SerializeField]
    Text _bestRunText = null;

    // Method

    public void UpdateInfoView(float heroMoveDistance)
    {
        int gold = PlayerDataManager.instance.HaveGold();

        int shieldDurability = 0;
        if (HeroManager.instance.SelectHeroObejct != null)
        {
            shieldDurability = HeroManager.instance.SelectHeroObejct.
                ModelControl.EquipItemSlotShield.GetEquipItemDurability();
        }

        _goldText.text = gold.ToString();
        _shieldText.text = shieldDurability.ToString();
        _bestRunText.text = heroMoveDistance.ToString("F0") + " m";
    }
}
