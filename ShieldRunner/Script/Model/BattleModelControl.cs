using UnityEngine;
using System.Collections;

public class BattleModelControl : ModelControl
{
    public BattleObject RootBattleObject { get { return _rootActionObject as BattleObject; } }

    [SerializeField]
    protected BattleObjectEquipItemSlot _equipItemSlotShield = null;
    public BattleObjectEquipItemSlot EquipItemSlotShield { get { return _equipItemSlotShield; } } 

    [SerializeField]
    protected BoxColliderChecker _colliderChecker_BattleObject = null;
    public BoxColliderChecker ColliderChecker_BattleObject { get { return _colliderChecker_BattleObject; } }

    [SerializeField]
    protected Transform _centerTransform = null;

    // Method

    #region Move

    public void MoveGetHitFly(Vector2 direction, float moveSpeed, float rotateSpeed, float scaleSpeed)
    {
        direction.Normalize();
        Vector2 resultMoveSpeed = direction * moveSpeed;

        // move
        Vector2 translateValue = new Vector2(resultMoveSpeed.x, resultMoveSpeed.y);
        RootBattleObject.TranslateAtSpaceWorld(translateValue);

        // rotation
        RootBattleObject.Translate(_centerTransform.localPosition);
        RootBattleObject.transform.Rotate(Vector3.forward, rotateSpeed);                                      
        RootBattleObject.Translate(_centerTransform.localPosition * -1f);

        // scale
        float localScaleX = RootBattleObject.transform.localScale.x;
        if (Mathf.Abs(localScaleX) >= 0.1f)
        {
            localScaleX -= localScaleX * scaleSpeed; 
        }

        float localScaleY = RootBattleObject.transform.localScale.y;
        if (Mathf.Abs(localScaleY) >= 0.1f)
        {
            localScaleY -= localScaleY * scaleSpeed; 
        }

        RootBattleObject.SetLocalScale(new Vector2(localScaleX, localScaleY));
    }

    #endregion
}
