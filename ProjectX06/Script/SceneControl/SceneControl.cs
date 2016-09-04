using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneHelper
{
    public const string DataLoadSceneName = "00_DataLoadScene";
    public const string TitleSceneName = "01_TitleScene";
    public const string MainSceneName = "02_MainScene";
    public const string StageSceneName = "03_StageScene";
    public const string SelectStageSceneName = "04_SelectStageScene";
    public const string SelectRunnerSceneName = "05_SelectRunnerScene";


    public static bool IsTitleScene()
    {
        return (SceneManager.GetActiveScene().name == TitleSceneName);
    }

    public static bool IsMainScene()
    {
        return (SceneManager.GetActiveScene().name == MainSceneName);
    }

    public static bool IsSelectStageScene()
    {
        return (SceneManager.GetActiveScene().name == SelectStageSceneName);
    }

    public static bool IsStageScene()
    {
        return (SceneManager.GetActiveScene().name == StageSceneName);
    }
}

public class SceneControl<T> : SingletonFindBehaviour<T> where T : MonoBehaviour
{
    public void LoadLevelScene(string sceneName)
    {
        Clear();

        SceneManager.LoadScene(sceneName);
    }

    protected virtual void Start()
    {
        CommonManager.instance.Init();

        CameraResolutionManager.instance.MainCameraResolutionUpdate();
    }

    protected virtual void Clear()
    {
    }
}
