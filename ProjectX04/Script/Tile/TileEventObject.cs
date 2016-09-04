using UnityEngine;
using System.Collections;

public class TileEventObject : MonoBehaviour {

	public virtual void EnterTile(ChaController cha)
	{
	}

	public virtual void EnterCompleteTile(ChaController cha)
	{
	}
		
	public virtual void LeaveTile(ChaController cha)
	{
	}
	
	public virtual bool IsReserveDestroy()
	{
		return false;
	}
	
	public virtual void DestroyObject()
	{
	}
}
