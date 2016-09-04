using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class UISelectStage_StageScrollSlot : UIScrollSlot
{
    [SerializeField]
    TrainerData _trainerData = null;
    public TrainerData TrainerData
    {
        get { return _trainerData; }
    }

    [SerializeField]
    GameObject _iconObject = null;

    [SerializeField]
    Image _selectMark = null;

    GameObject _trainerIcon = null;

    [SerializeField]
    List<Image> _clearedStarList = new List<Image>();


    public Action<UISelectStage_StageScrollSlot> _clickEvent = null;


    public void Init(TrainerData trainerData)
    {
        if (trainerData == null)
            return;

        _trainerData = trainerData;

        GameObject prefab = Resources.Load("Prefab/Icon/Trainer/" + trainerData._icon) as GameObject;
        if (prefab == null)
            return;
        
        _trainerIcon = Instantiate(prefab).gameObject;
        if (_trainerIcon == null)
            return;

        _trainerIcon.transform.SetParent(_iconObject.transform);
        _trainerIcon.transform.localPosition = Vector3.zero;
        _trainerIcon.transform.localRotation = Quaternion.identity;
    }

    public void OnClick()
    {
        if (_clickEvent != null)
        {
            _clickEvent(this);
        }
    }

    public void SelectMark(bool isSelect)
    {
        _selectMark.enabled = isSelect;
    }

    public void UpdateClearedStar()
    {
        for (int index = 0; index < _clearedStarList.Count; ++index)
        {
            _clearedStarList[index].gameObject.SetActive(false);
        }
    }
}
