using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum SceneControlType
{
    None,
    Title,
    Main,
    Stage,
    BlockMaker,
}

public class SceneHelper : SingletonNew<SceneHelper>
{
	public const string TitleSceneName = "01_TitleScene";
	public const string MainSceneName = "02_MainScene";
	public const string StageSceneName = "03_StageScene";

    public static SceneControlType CurrentSceneType { get; set; }

    // Method

	public static bool IsToolScene()
	{
        #if UNITY_EDITOR
        return CurrentSceneType == SceneControlType.BlockMaker;
        #elif
        return false;
        #endif
	}

    public static bool IsInGame()
    {
        return ! IsToolScene();
    }

	public static bool IsStageScene()
	{
        return (CurrentSceneType == SceneControlType.Stage);
	}
}

public abstract class SceneControl<T> : SingletonFindBehaviour<T> where T : MonoBehaviour
{
    protected virtual SceneControlType SceneControlType
    {
        get { return SceneControlType.None; }
    }

    protected virtual bool IsAsynkInit { get { return false; } }

    // Method

	public void LoadLevelScene(string sceneName)
	{
		Clear();

		SceneManager.LoadScene(sceneName);
	}

	protected virtual void Start()
	{
        SceneHelper.CurrentSceneType = SceneControlType;

        if (IsAsynkInit == true)
        {
            CommonManager.instance.InitAsynk(LoadingAction, CompleteAction);
            return;
        }

        CommonManager.instance.Init();
	}

	protected virtual void Clear()
	{
	}

    public virtual void LoadingAction(int percent)
    {
    }

    public virtual void CompleteAction()
    {
    }
}
