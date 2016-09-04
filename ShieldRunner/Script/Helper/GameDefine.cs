using UnityEngine;
using System.Collections;

public enum Direction
{
	Right,
	Left,
	Up,
	Down,
}

public enum BattleTeam
{
	None = -1,
	HeroTeam,
	MonsterTeam,
}

public enum BattleObjectType
{
	None = -1,
	Hero,
	Monster,
	BossMonster,
}

public enum BattleAttackType
{
	None = -1,
	Melee,
	Range,
	Magic,
}

public enum BattleStatType
{
	None = -1,

	Hp,
	MeleeAttack,
	RangeAttack,
	MagicAttack,
}

public enum EquipItemMainType
{
	None = -1,

	Sword,
	Bow,
	Staff,

	Hat,
	Shield,
}

public enum PickupItemType
{
    None = -1,

    Gold,
}
