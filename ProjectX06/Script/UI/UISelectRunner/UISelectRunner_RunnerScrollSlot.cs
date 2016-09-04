using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UISelectRunner_RunnerScrollSlot : UIScrollSlot
{
    [SerializeField]
    public GameObject _iconObject = null;

    [SerializeField]
    public Image _selectMark = null;

    RunnerData _runnerData = null;
    public RunnerData RunnerData { get { return _runnerData; } }

    GameObject _runnerIcon = null;

    public Action<UISelectRunner_RunnerScrollSlot> _slotClickEvent = null;


    public void Init(RunnerData runnerData)
    {
        if (runnerData == null)
            return;

        _runnerData = runnerData;

        GameObject prefab = Resources.Load("Prefab/Icon/Runner/" + runnerData._runnerGrade[0]._icon) as GameObject;
        if (prefab == null)
            return;
        
        _runnerIcon = Instantiate(prefab).gameObject;
        if (_runnerIcon == null)
            return;

        _runnerIcon.transform.SetParent(_iconObject.transform);
        _runnerIcon.transform.localPosition = Vector3.zero;
        _runnerIcon.transform.localRotation = Quaternion.identity;

        ActiveSelectMark(false);
    }

    public void ActiveSelectMark(bool active)
    {
        _selectMark.gameObject.SetActive(active);
    }

    public void OnRunnerIconClick()
    {
        if (_slotClickEvent != null)
        {
            _slotClickEvent(this);
        }
    }
}
