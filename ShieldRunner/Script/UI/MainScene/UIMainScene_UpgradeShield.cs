using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIMainScene_UpgradeShield : MonoBehaviour
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

            if (infoData._upgradeEquipItemId <= 0)
            {
                isActive = false;
                break;
            }

            costText = infoData._cost.ToString();

            if (infoData._cost > PlayerDataManager.instance.HaveGold())
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

    public void OnUpgradeButton()
    {
        int lastEquipShiledId = PlayerDataManager.instance.LastEquipShieldId();

        UpgradeInfoData upgradeInfoData = UpgradeDataManager.instance.InfoDataAtEquipItemId(lastEquipShiledId);
        if (upgradeInfoData == null)
            return;

        if (upgradeInfoData._upgradeEquipItemId <= 0)
            return;

        if (upgradeInfoData._cost > PlayerDataManager.instance.HaveGold())
            return;

        if (MainSceneControl.instance == null)
            return;

        PlayerDataManager.instance.DecreaseGold(upgradeInfoData._cost);
        PlayerDataManager.instance.SetLastEquipShieldId(upgradeInfoData._upgradeEquipItemId);

        MainSceneControl.instance.UpdateMainSceneUI();
    }
}
