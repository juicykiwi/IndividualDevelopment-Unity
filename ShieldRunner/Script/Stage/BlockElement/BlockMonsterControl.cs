using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class BlockMonsterControl : BlockElementControl<CreationBattleObjectInfo, MonsterObject>
{
    protected override MonsterObject CreateElement(CreationBattleObjectInfo creationInfo)
    {
        if (creationInfo == null)
            return null;

        MonsterObject monsterObject = MonsterManager.instance.CreateMonster(creationInfo._uniqueId);
        if (monsterObject == null)
            return null;

        monsterObject.transform.SetParent(MonsterManager.instance.transform);

        float posX = _parentBlockObject.transform.position.x + creationInfo._pos.x;
        float posY = _parentBlockObject.transform.position.y + creationInfo._pos.y;
        monsterObject.SetPos(GameObjectHelper.RoundPosition2D(new Vector2(posX, posY)));

        monsterObject.SetLookAt(Direction.Left);

        return monsterObject;
    }

    public override void Add(MonsterObject tObject)
    {
        if (tObject == null)
            return;

        tObject.transform.SetParent(MonsterManager.instance.transform);
        tObject.SetPos(GameObjectHelper.RoundPosition2D(tObject.transform.position));
        tObject.SetLookAt(Direction.Left);

        _objectList.Add(tObject);
    }

    public override void Remove(MonsterObject tObject)
    {
        if (tObject == null)
            return;

        _objectList.Remove(tObject);
        tObject.RemoveActionObject();
    }
}
