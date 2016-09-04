using UnityEngine;
using System.Collections;

public class HeroObjectFactory : BattleObjectFactory
{
    // Method

    public override BattleObject CreateBattleObject(int uniqueId)
    {
        return NewBattleObject<HeroObject>(uniqueId);
    }

    public HeroObject CreateHeroObject(int uniqueId, bool inGame)
    {
        HeroObject heroObject = CreateBattleObject(uniqueId) as HeroObject;
        if (heroObject == null)
            return null;

        heroObject.CreatedId = HeroObject.GetNextCreatedHeroId;

        if (inGame == true)
        {
            heroObject.InitAI();
        }

        return heroObject;
    }
}
