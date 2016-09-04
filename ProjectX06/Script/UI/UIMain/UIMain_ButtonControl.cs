using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMain_ButtonControl : MonoBehaviour
{
    [SerializeField]
    Button _startButton = null;

    [SerializeField]
    Button _characterButton = null;

    [SerializeField]
    Button _shopButton = null;


    public void OnStartButton()
    {
        MainSceneControl.instance.LoadLevelScene(SceneHelper.StageSceneName);
    }

    public void OnCharacterButton()
    {
        MainSceneControl.instance.LoadLevelScene(SceneHelper.SelectRunnerSceneName);
    }

    public void OnShopButton()
    {
    }
}
