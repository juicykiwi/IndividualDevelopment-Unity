using UnityEngine;
using System.Collections;

public class MonsterObject : BattleObject, IGameObjectPool, IBlockElement
{
    Vector3 ResetPos = new Vector3(-10f, -10f, 0f);

    public override BattleTeam BattleTeam { get { return BattleTeam.MonsterTeam; } }

    public BlockElementType ElementType { get { return BlockElementType.Monster; } }

    // Method

	#region ActionObject override

    public override void Reset()
    {
        _aiContorl.SetAIStateWithType(AIStateType.Spawn);
        transform.localRotation = Quaternion.identity;
        SetLocalScale(_baseScale);
        SetLookAt(Direction.Left);
    }

    public override void RemoveActionObject()
	{
        if (SceneHelper.IsInGame() == true)
        {
            MonsterManager.instance.MonsterPool.Enqueue(_infoData._uniqueId, this);
        }
        else
        {
            base.RemoveActionObject();
        }
	}

	#endregion

    #region MonoBehaviour event

	protected void FixedUpdate()
	{
		Vector2 pos = new Vector2(transform.position.x, transform.position.y);
		if (CameraManager.instance.IsNeedDestroyByObjectPos(pos) == false)
			return;

        RemoveActionObject();
	}

	#endregion

    #region IGameObjectPool

    public void ReadyEnqueue()
    {
        gameObject.SetActive(false);
        transform.position = ResetPos;
        transform.SetParent(MonsterManager.instance.transform);
    }

    public void ReadyDequeue()
    {
        gameObject.SetActive(true);
        Reset();
    }

    #endregion
}
