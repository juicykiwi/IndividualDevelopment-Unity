using UnityEngine;
using System.Collections;

public class MonsterObjectFactory : BattleObjectFactory
{
    // Created ID
    static int _createdMonsterIdLast = 0;
    public static int GetNewCreatedMonsterId { get { return ++_createdMonsterIdLast; } }

    // Method

    public override BattleObject CreateBattleObject(int uniqueId)
    {
        return NewBattleObject<MonsterObject>(uniqueId);
    }

    public MonsterObject CreateMonsterObject(int uniqueId, bool inGame)
    {
        MonsterObject monsterObject = CreateBattleObject(uniqueId) as MonsterObject;
        if (monsterObject == null)
            return null;
        
        monsterObject.CreatedId = GetNewCreatedMonsterId;
        monsterObject.SetPos(new Vector3(0f, 0f, monsterObject.CreatedId));

        if (inGame == true)
        {
            monsterObject.InitAI();
        }
        else
        {
            if (monsterObject.VerticalMoveControl != null)
                monsterObject.VerticalMoveControl.enabled = false;
        }

        return monsterObject;
    }
}
