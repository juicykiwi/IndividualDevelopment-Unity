using UnityEngine;
using System.Collections;

public class RunningMachineOutLineCollider : MonoBehaviour
{
    [SerializeField]
    float _respawnDelayTime = 0f;


    public void OnTriggerEnter2D(Collider2D coll)
    {
        Runner runner = coll.GetComponent<Runner>();
        if (runner == null)
            return;

        switch (runner.RunnerState)
        {
            case RunnerState.Spawn:
            case RunnerState.Out:
                return;

            default:
                break;
        }

        runner.RunnerState = RunnerState.Out;
        StageSceneControl.instance.Invoke("RespawnRunner", _respawnDelayTime);
    }
}
