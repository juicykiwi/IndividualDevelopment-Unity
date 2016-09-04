using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UISelectStage_StageScrollView : UIScrollView
{
    [SerializeField]
    UISelectStage_StageScrollSlot _selectedSlot = null;
    public UISelectStage_StageScrollSlot SelectedSlot { get { return _selectedSlot; } }

    public void AddTrainerSlot(TrainerData trainerData)
    {
        if (trainerData == null)
            return;

        UISelectStage_StageScrollSlot slot = CreateScrollSlot() as UISelectStage_StageScrollSlot;
        if (slot == null)
            return;

        slot.SelectMark(false);
        slot.UpdateClearedStar();
        slot.Init(trainerData);
        slot._clickEvent += OnSelectedSlot;

        AddContent(slot);
    }

    public void OnSelectedSlot(UISelectStage_StageScrollSlot slot)
    {
        if (slot == null)
            return;

        if (_selectedSlot != null)
        {
            _selectedSlot.SelectMark(false);
        }

        _selectedSlot = slot;
        _selectedSlot.SelectMark(true);
    }
}


#if UNITY_EDITOR

[CanEditMultipleObjects]
[CustomEditor(typeof(UISelectStage_StageScrollView))]
public class UISelectStage_StageScrollViewEditor : UIScrollViewEditor<UISelectStage_StageScrollSlot>
{
}

#endif
