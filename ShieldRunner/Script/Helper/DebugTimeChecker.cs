using UnityEngine;
using System.Collections;

public class DebugTimeChecker : Singleton<DebugTimeChecker>
{
    float _time = 0;

    // Method

    public void Start()
    {
        _time = Time.realtimeSinceStartup;
    }

    public void End(string text, float baseTime)
    {
        if (Time.realtimeSinceStartup - _time >= baseTime)
            Debug.LogFormat("{0} : {1:F5}", text, Time.realtimeSinceStartup - _time);
    }
}
