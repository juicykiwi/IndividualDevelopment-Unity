using UnityEngine;
using System.Collections;

public class CommonManager : Singleton<CommonManager>
{
    [SerializeField]
    bool _alreadyInit = false;

    public void Init()
    {
        if (_alreadyInit == true)
            return;
        
        CameraResolutionManager.instance.transform.SetParent(transform);

        TouchManager.instance.transform.SetParent(transform);

        /* Data manager */

        UserDataManager.instance.transform.SetParent(transform);
        UserDataManager.instance.Init();

        RunnerDataManager.instance.transform.SetParent(transform);
        RunnerDataManager.instance.Init();

        TrainerDataManager.instance.transform.SetParent(transform);
        TrainerDataManager.instance.Init();

        /* Manager */

        CharacterManager.instance.transform.SetParent(transform);

        StagePlayManager.instance.transform.SetParent(transform);

        _alreadyInit = true;
    }
}
