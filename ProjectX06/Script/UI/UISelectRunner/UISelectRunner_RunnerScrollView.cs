using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UISelectRunner_RunnerScrollView : UIScrollView
{
    UISelectRunner_RunnerScrollSlot _selectedSlot = null;


    public void AddRunnerSlot(RunnerData runnerData)
    {
        if (runnerData == null)
            return;

        UISelectRunner_RunnerScrollSlot slot = CreateScrollSlot() as UISelectRunner_RunnerScrollSlot;
        if (slot == null)
            return;

        slot.Init(runnerData);
        slot._slotClickEvent += OnSlotClick;

        if (runnerData._id == UserDataManager.instance.GetLastSelectedRunnerID())
        {
            OnSlotClick(slot);
        }

        AddContent(slot);
    }

    public void OnSlotClick(UISelectRunner_RunnerScrollSlot slot)
    {
        if (_selectedSlot != null)
        {
            _selectedSlot.ActiveSelectMark(false);
        }

        _selectedSlot = slot;
        _selectedSlot.ActiveSelectMark(true);

        SelectRunnerSceneControl.instance.SpawnSelectedRunner(_selectedSlot.RunnerData);
    }
}



#if UNITY_EDITOR

[CanEditMultipleObjects]
[CustomEditor(typeof(UISelectRunner_RunnerScrollView))]
public class UISelectRunner_RunnerScrollViewEditor : UIScrollViewEditor<UISelectRunner_RunnerScrollSlot>
{
}

#endif

