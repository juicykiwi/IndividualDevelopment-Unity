using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMainScene_EnchantShield : MonoBehaviour
{
    [SerializeField]
    Button _button = null;

    [SerializeField]
    Text _goldCostText = null;

    // Method

    public void CheckButtonActive()
    {
        bool isActive = true;
        bool isInteractable = true;
        string costText = "-";

        do
        {
            int lastEquipShieldId = PlayerDataManager.instance.LastEquipShieldId();
            if (lastEquipShieldId <= 0)
            {
                isActive = false;
                break;
            }
                
            UpgradeInfoData infoData = UpgradeDataManager.instance.InfoDataAtEquipItemId(lastEquipShieldId);
            if (infoData == null)
            {
                isActive = false;
                break;
            }

            // 마지막 업그레이드 된 방패만 인챈트 
            if (infoData._upgradeEquipItemId > 0)
            {
                isActive = false;
                break;
            }

            int enchantCost = PlayerDataManager.instance.NextEnchantCost();
            costText = enchantCost.ToString();

            if (enchantCost > PlayerDataManager.instance.HaveGold())
            {
                isInteractable = false;
                break;
            }

        } while (false);

        if (isActive == true)
        {
            _button.interactable = isInteractable;
            _goldCostText.text = costText;
        }

        gameObject.SetActive(isActive);
    }

    public void OnEnchantButton()
    {
        int enchantCost = PlayerDataManager.instance.NextEnchantCost();
        if (enchantCost > PlayerDataManager.instance.HaveGold())
            return;

        if (MainSceneControl.instance == null)
            return;

        PlayerDataManager.instance.DecreaseGold(enchantCost);
        PlayerDataManager.instance.Enchant();
            
        MainSceneControl.instance.UpdateMainSceneUI();
    }
}
