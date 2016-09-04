using UnityEngine;
using System.Collections;

public enum Direction
{
	None,
	Up,
	Down,
	Right,
	Left,
	Max,
}

public enum Axis
{
	None,
	X,
	Y,
	Z,
	All,
}

public enum Team
{
	None,
	UserTeam,
	EnemyAITeam,
}

public enum CheckTeamType
{
	None,
	Ally,
	Enemy,
}

public class GameHelper
{
	public static Direction GetRandomDirection(bool isIncludeNone)
	{
		int min = (int)Direction.Up;
		if (isIncludeNone == true)
		{
			min = (int)Direction.None;
		}

		int max = (int)Direction.Max;

		Direction direction = (Direction)(Random.Range(min, max));
		return direction;
	}

	public static Direction GetDirectionWithPos(Vector2 centerPos, Vector2 targetPos)
	{
		float angle = Vector2.Angle(Vector2.up, (targetPos - centerPos));

		if (angle <= 45.0f)
		{
			return Direction.Up;
		}
		else if (angle <= 135.0f)
		{
			if (targetPos.x - centerPos.x >= 0)
			{
				return Direction.Right;
			}
			else
			{
				return Direction.Left;
			}
		}
		else
		{
			return Direction.Down;
		}
	}

	public static Vector2 GetVectorWithDirection(Direction direction)
	{
		switch (direction)
		{
		case Direction.Up:
			return Vector2.up;

		case Direction.Down:
			return Vector2.up * -1f;

		case Direction.Right:
			return Vector2.right;

		case Direction.Left:
			return Vector2.right * -1f;

		default:
			break;
		}

		return Vector2.zero;
	}

	public static Vector2 RoundPos(Vector2 pos)
	{
		return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
	}

	public static Vector2 RoundPos(Vector3 pos)
	{
		return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
	}
}
