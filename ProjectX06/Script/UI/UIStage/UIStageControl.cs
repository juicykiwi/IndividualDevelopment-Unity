using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIStageControl : SingletonFindBehaviour<UIStageControl>
{
    [SerializeField]
    UIStage_TopView _uiTopView = null;
    public UIStage_TopView UITopView { get { return _uiTopView; } } 

    [SerializeField]
    UIStage_Countdown _uiStage_countdown = null;
    public UIStage_Countdown UIStage_Countdown { get { return _uiStage_countdown; } }

    [SerializeField]
    Button _uiGoMainButton = null;
    public Button UIGoMainButton { get { return _uiGoMainButton; } }

    [SerializeField]
    Slider _uiStageLevelSlider = null;
    public Slider UIStageLevelSlider { get { return _uiStageLevelSlider; } }

    [SerializeField]
    UIStage_WaterBottleControl _uiWaterBottleControl = null;
    public UIStage_WaterBottleControl UiWaterBottleControl { get { return _uiWaterBottleControl; } }


    public void Init()
    {
        _uiTopView.UpdateStarCoinCount();

        _uiGoMainButton.gameObject.SetActive(false);
        _uiStageLevelSlider.value = _uiStageLevelSlider.maxValue;
    }


    public void ActiveGoMainButton(bool active, float time)
    {
        StartCoroutine(ActiveGoMainButtonCoroutine(active, time));
    }

    IEnumerator ActiveGoMainButtonCoroutine(bool active, float time)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            time -= Time.fixedDeltaTime;

            if (time <= 0f)
                break;
        }

        _uiGoMainButton.gameObject.SetActive(active);
    }

    public void OnGoMainButton()
    {
        StageSceneControl.instance.LoadLevelScene(SceneHelper.MainSceneName);
    }
}
