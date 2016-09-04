using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleSceneControl : SceneControl<TitleSceneControl>
{
	[SerializeField]
	float _dataLoadPreogress = 0f;
	public float DataLoadPreogress { get { return _dataLoadPreogress; } } 

    protected override SceneControlType SceneControlType
    {
        get { return SceneControlType.Title; }
    }

    protected override bool IsAsynkInit { get { return true; } }

	// Method

	protected override void Start()
	{
        base.Start();
	}

    public override void LoadingAction(int percent)
    {
        Debug.Log("LoadingAction : " + percent + "% / " + Time.realtimeSinceStartup);
    }

    public override void CompleteAction()
    {
        LoadLevelScene(SceneHelper.MainSceneName);
    }
}
