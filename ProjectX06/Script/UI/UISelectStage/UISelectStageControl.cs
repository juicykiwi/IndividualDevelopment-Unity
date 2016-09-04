using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UISelectStageControl : SingletonFindBehaviour<UISelectStageControl>
{
    [SerializeField]
    UISelectStage_StageScrollView _uiSelectStageScrollView = null;
    public UISelectStage_StageScrollView UISelectStageScrollView
    {
        get { return _uiSelectStageScrollView; } 
    }


    public void Init()
    {
        SettingSelectStageScrollView();
    }

    void SettingSelectStageScrollView()
    {
        List<TrainerData>.Enumerator trainerEnumerator = 
            TrainerDataManager.instance.GetTrainerDataListEnumerator();

        while (trainerEnumerator.MoveNext() == true)
        {
            if (trainerEnumerator.Current == null)
                continue;

            _uiSelectStageScrollView.AddTrainerSlot(trainerEnumerator.Current);
        }
    }
}
