using UnityEngine;
using System.Collections;

public class StagePlayManager : Singleton<StagePlayManager>
{
    [SerializeField]
    int _startTrainerId = 0;
    public int StartTrainerId
    {
        get { return _startTrainerId; } 
        set { _startTrainerId = value; }
    }
}
