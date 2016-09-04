using UnityEngine;
using System.Collections;

public class RunningMachineFloorCollider : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D coll)
    {
        Runner runner = coll.collider.GetComponent<Runner>();
        if (runner == null)
            return;
        
        runner.SetLanded(true);
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        Runner runner = coll.collider.GetComponent<Runner>();
        if (runner == null)
            return;

        runner.SetLanded(false);
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        Runner runner = coll.GetComponent<Runner>();
        if (runner == null)
            return;

        runner.SetLanded(true);
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        Runner runner = coll.GetComponent<Runner>();
        if (runner == null)
            return;

        runner.SetLanded(false);
    }
}
