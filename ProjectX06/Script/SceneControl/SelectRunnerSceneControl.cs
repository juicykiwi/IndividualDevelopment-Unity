using UnityEngine;
using System.Collections;

public class SelectRunnerSceneControl : SceneControl<SelectRunnerSceneControl>
{
    [SerializeField]
    Vector3 _spawnRunnerPos = Vector3.zero;

    Runner _selectedRunner = null;


    protected override void Start()
    {
        base.Start();

        UISelectRunnerControl.instance.Init();
    }

    public void SpawnSelectedRunner(RunnerData runnerData)
    {
        if (runnerData == null)
            return;

        Runner newRunner = Runner.CreateRunner(runnerData);
        if (newRunner == null)
            return;

        if (_selectedRunner != null)
        {
            Destroy(_selectedRunner.gameObject);
        }

        _selectedRunner = newRunner;
        _selectedRunner.transform.position = _spawnRunnerPos;
    }
}
