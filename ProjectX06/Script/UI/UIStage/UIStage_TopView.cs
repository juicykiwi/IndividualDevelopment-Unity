using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIStage_TopView : MonoBehaviour
{
    [SerializeField]
    Text _starCoin = null;


    public void UpdateStarCoinCount()
    {
        _starCoin.text = string.Format("{0}", UserDataManager.instance.GetUserData()._starCoin);
    }
}
