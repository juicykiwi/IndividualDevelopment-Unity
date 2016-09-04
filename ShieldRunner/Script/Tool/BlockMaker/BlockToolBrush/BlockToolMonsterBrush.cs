using UnityEngine;
using System.Collections;

public class BlockToolMonsterBrush : IBlockToolBrush
{
    int _monsterId = 0;
    public int ElementId
    {
        get { return _monsterId; }
        set { _monsterId = value; }
    }

    public IBlockElement Draw(Vector2 pos)
    {
        MonsterObject monster = MonsterManager.instance.CreateMonster(ElementId);
        if (monster == null)
            return null;

        monster.transform.position = pos;
        return monster;
    }
}
