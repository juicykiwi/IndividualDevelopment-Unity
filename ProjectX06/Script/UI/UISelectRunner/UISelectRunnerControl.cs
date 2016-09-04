using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UISelectRunnerControl : SingletonFindBehaviour<UISelectRunnerControl>
{
    [SerializeField]
    UISelectRunner_RunnerScrollView _uiSelectRunnerScrollView = null;


    public void Init()
    {
        SettingSelectRunnerScrollView();
    }

    void SettingSelectRunnerScrollView()
    {
        List<RunnerData>.Enumerator runnerEnumerator = 
            RunnerDataManager.instance.GetRunnerDataListEnumerator();

        while (runnerEnumerator.MoveNext() == true)
        {
            if (runnerEnumerator.Current == null)
                continue;

            _uiSelectRunnerScrollView.AddRunnerSlot(runnerEnumerator.Current);
        }
    }

    public void OnBackButton()
    {
        SelectRunnerSceneControl.instance.LoadLevelScene(SceneHelper.MainSceneName);
    }
}
