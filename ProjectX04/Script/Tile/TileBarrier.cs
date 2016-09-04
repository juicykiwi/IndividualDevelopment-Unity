using UnityEngine;
using System.Collections;

public class TileBarrier : TileBase {

	// Method

	public override TileType GetTileType()
	{
		return TileType.Barrier;
	}
	
	public override bool IsEnableMoveTile()
	{
		return false;
	}

	public override bool IsOverlay(TileType checkTileType)
	{
		bool isOverlay = false;

		switch (checkTileType)
		{
		case TileType.Barrier:
			break;

		default:
			break;
		}

		return isOverlay;
	}
}
