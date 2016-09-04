using UnityEngine;
using System.Collections;

public partial class TileObject : MonoBehaviour, IGameObjectPool, IBlockElement
{
    Vector3 ResetPos = new Vector3(-10f, -10f, 0f);
	
	[SerializeField]
	TileObjectInfoData _infoData = null;
	public TileObjectInfoData InfoData
	{
		get { return _infoData; }
		set { _infoData = value; }
	}

	[SerializeField]
	int _createdTileId = 0;
	public int CreatedTileId
	{
		get { return _createdTileId; }
		set { _createdTileId = value; }
	}

    public BlockElementType ElementType { get { return BlockElementType.Tile; } }

    // Method

    public void Remove()
    {
        if (SceneHelper.IsInGame() == true)
        {
            TileManager.instance.TilePool.Enqueue(_infoData._uniqueId, this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

	public void SetLocalPos(Vector2 pos2D)
	{
		SetLocalPos(new Vector3(pos2D.x, pos2D.y, transform.localPosition.z));
	}

	public void SetLocalPos(Vector3 pos3D)
	{
		transform.localPosition = pos3D;
	}

    public float TopPointY()
    {
        Rect rect = GameObjectHelper.RectByTransform(transform);
        return rect.yMax;
    }

    #region IGameObjectPool

    public void ReadyEnqueue()
    {
        gameObject.SetActive(false);
        transform.position = ResetPos;
        transform.SetParent(TileManager.instance.transform);
    }

    public void ReadyDequeue()
    {
        gameObject.SetActive(true);
    }

    #endregion
}





