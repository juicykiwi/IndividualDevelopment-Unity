using UnityEngine;
using System.Collections;

public class TileGround : TileBase {

	// Method

	public override TileType GetTileType()
	{
		return TileType.Ground;
	}

	public override bool IsEnableMoveTile()
	{
		return true;
	}
}
