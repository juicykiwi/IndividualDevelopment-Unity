using UnityEngine;
using System.Collections;

public class UISelectStage_TopView : MonoBehaviour
{
    public void OnBackButton()
    {
        SelectStageSceneControl.instance.LoadLevelScene(SceneHelper.MainSceneName);
    }

    public void OnStartButton()
    {
        TrainerData trainerData = null;

        if (UISelectStageControl.instance.UISelectStageScrollView.SelectedSlot != null)
        {
            trainerData = UISelectStageControl.instance.UISelectStageScrollView.SelectedSlot.TrainerData;
        }

        if (trainerData == null)
        {
            Debug.Log("trainerData is null.");
            return;
        }

        Debug.LogFormat("trainerData._id : {0}", trainerData._id);

        StagePlayManager.instance.StartTrainerId = trainerData._id;
        SelectStageSceneControl.instance.LoadLevelScene(SceneHelper.StageSceneName);
    }
}
