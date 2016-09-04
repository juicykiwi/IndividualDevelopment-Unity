using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunningMachineRailControl : MonoBehaviour
{
    [SerializeField]
    float _railSize = 0.9f;

    [SerializeField]
    float _nextRailPosOffset = 2.7f;

    [SerializeField]
    float _railSpeed = 0f;

    [SerializeField]
    List<RunningMachineRail> _railList = new List<RunningMachineRail>();


    void Update()
    {
        if (_railSpeed == 0f)
            return;

        for (int index = 0; index < _railList.Count; ++index)
        {
            _railList[index].transform.Translate(_railSpeed * -1f * Time.deltaTime, 0f, 0f);

            if (_railList[index].transform.localPosition.x < _railSize * -1f)
            {
                _railList[index].transform.Translate(_nextRailPosOffset, 0f, 0f);
            }
        }
    }

    public void SetRailSpeed(float speed)
    {
        _railSpeed = speed;
    }

//    public void OnRailForce(float force)
//    {
//        if (force == 0f)
//            return;
//
//        for (int index = 0; index < _railList.Count; ++index)
//        {
//            _railList[index].OnRailForce(force);
//
//            if (_railList[index].transform.localPosition.x < _railSize * -1f)
//            {
//                _railList[index].transform.Translate(_nextRailPosOffset * 10f, 0f, 0f);
//            }
//        }
//    }
}
