using UnityEngine;
using System.Collections;

public class RunningMachineStandCollider : MonoBehaviour
{
    [SerializeField]
    Vector2 _bounceForce = Vector2.zero;


    void OnCollisionEnter2D(Collision2D coll)
    {
        Runner runner = coll.collider.GetComponent<Runner>();
        if (runner == null)
            return;

        runner.OnRunningMachineStandBoune(_bounceForce);
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        Runner runner = coll.GetComponent<Runner>();
        if (runner == null)
            return;

        runner.OnRunningMachineStandBoune(_bounceForce);
    }
}
