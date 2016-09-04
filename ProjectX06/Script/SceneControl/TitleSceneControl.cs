using UnityEngine;
using System.Collections;

public class TitleSceneControl : SceneControl<TitleSceneControl>
{
    protected override void Start()
    {
        base.Start();

        Invoke("GoMainScene", 1f);
    }

    void GoMainScene()
    {
        LoadLevelScene(SceneHelper.MainSceneName);
    }
}
