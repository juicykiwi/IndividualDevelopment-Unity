using UnityEngine;
using System.Collections;

public class AIMessage
{
}

public class AIMessageMove : AIMessage
{
	public Direction _direction = Direction.None;
	public Vector2 _movePos = Vector2.zero;
}
