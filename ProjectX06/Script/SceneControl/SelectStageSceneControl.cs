using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectStageSceneControl : SceneControl<SelectStageSceneControl>
{
    protected override void Start()
    {
        base.Start();

        UISelectStageControl.instance.Init();
    }
}
