using UnityEngine;
using System.Collections;

public enum TileType
{
	None,
	
	// Base tile.
	Ground,
	Barrier,
	Environment,
	
	// over base tile.
	HidingPlace,
	
	// start position tile.
	StartPos,
	
	// end position tile.
	EndPos,
	
	// Enemy Spawn tile.
	EnemySpawn,
	
	// Do event.
	DoEvent,
	
	// Move board.
	MoveBoard,
}

public class TileBase : TileEventObject
{

	protected int _tileId = 0;
	public int tileId { set { _tileId = value; } }

	public bool _isUnderCha = false;
	public int _tilePathWeight = 1;

	public Vector2 GetPos()
	{
		return GameHelper.RoundPos(transform.position);
	}

	public virtual TileType GetTileType()
	{
		return TileType.None;
	}

	public virtual bool IsEnableMoveTile()
	{
		return false;
	}
	
	public virtual bool IsOverlay(TileType checkTileType)
	{
		return false;
	}
	
	public virtual TileInfoData CreateTileInfoData()
	{
		TileInfoData tileInfo = new TileInfoData();
		tileInfo._id = _tileId;

		Vector2 roundPos = GameHelper.RoundPos(transform.position);
		tileInfo._postion = new Vector3(roundPos.x, roundPos.y, transform.position.z);
		
		char[] split = { '(', ')' };
		string[] prefabName = gameObject.name.Split(split, System.StringSplitOptions.RemoveEmptyEntries);
		if (prefabName.Length <= 0)
			return null;
		
		tileInfo._tilePrefab = prefabName[0];
		
		return tileInfo;
	}
	
	public virtual void InitWithInfoData(TileInfoData tileInfo)
	{
		_tileId = tileInfo._id;
		transform.position = tileInfo._postion;
	}

	public override void EnterTile(ChaController cha)
	{
	}

	public override void EnterCompleteTile(ChaController cha)
	{
	}

	public override void LeaveTile(ChaController cha)
	{
	}
	
	public override bool IsReserveDestroy()
	{
		return false;
	}

	public override void DestroyObject()
	{
		Destroy(gameObject);
	}
}
