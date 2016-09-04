using UnityEngine;
using System.Collections;

public class PickupItemObject : ActionObject, IGameObjectPool, IBlockElement
{
    Vector3 ResetPos = new Vector3(-10f, -10f, 0f);

    [SerializeField]
    PickupItemInfoData _infoData = null;
    public PickupItemInfoData InfoData
    {
        get { return _infoData; }
        set { _infoData = value; }
    }

    [SerializeField]
    int _createdId = 0;
    public int CreatedId
    {
        get { return _createdId; }
        set { _createdId = value; }
    }

    public BlockElementType ElementType { get { return BlockElementType.PickupItem; } }

    // Method

    #region MonoBehaviour event

    void FixedUpdate()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        if (CameraManager.instance.IsNeedDestroyByObjectPos(pos) == false)
            return;

        RemoveActionObject();
    }

    #endregion


    #region Remove

    public override void RemoveActionObject()
    {
        if (SceneHelper.IsInGame() == true)
        {
            PickupItemManager.instance.PickupItemPool.Enqueue(InfoData._uniqueId, this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion


    #region Battle action

    public override bool IsEnableGetHit()
    {
        return true;
    }

    public override void GetHit(BattleObject hitter)
    {
        PlayerDataManager.instance.IncreaseGold(_infoData._value);
        RemoveActionObject();
    }

    #endregion

    #region IGameObjectPool

    public void ReadyEnqueue()
    {
        gameObject.SetActive(false);
        transform.position = ResetPos;
        transform.SetParent(PickupItemManager.instance.transform);
    }

    public void ReadyDequeue()
    {
        gameObject.SetActive(true);
    }

    #endregion
}
