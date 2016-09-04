using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ModelControl : MonoBehaviour
{
	[SerializeField]
	protected ActionObject _rootActionObject = null;
    public  ActionObject RootActionObject { get { return _rootActionObject; } }

    [SerializeField]
    protected BoxColliderChecker _colliderChecker_tile = null;

    // Method

	// Set position
	public void SetPos(Vector2 newPos)
	{
        _rootActionObject.SetPos(newPos);
	}
        
	#region Move

	public void MoveFront(float speed)
	{
        Vector2 translateValue = new Vector3(speed * RootActionObject.LookAt.x, 0f);
        _rootActionObject.Translate(translateValue);
	}

	public void MoveDown(float speed)
	{
        if (speed < 0f)
        {
            TileObject tileObject = TopPosTile();
            if (tileObject != null)
            {
                Vector2 checkerPos = _colliderChecker_tile.transform.position;
                if (checkerPos.y >= tileObject.TopPointY())
                {
                    if (checkerPos.y + speed <= tileObject.TopPointY())
                    {
                        speed = tileObject.TopPointY() - checkerPos.y;
                    }
                }
            }
        }

		Vector2 translateValue = new Vector2(0f, speed);
        _rootActionObject.Translate(translateValue);

        if (SceneHelper.IsStageScene() == true)
        {
            if (StageSceneControl.instance.TestMode == true)
            {
                if (_rootActionObject.GetPos().y < 0.5f)
                {
                    _rootActionObject.Translate(new Vector2(0f, 3f));
                }
            }
        }
	}

	#endregion

	public bool IsGround()
	{
        TileObject tileObject = TopPosTile();
        if (tileObject == null)
            return false;

        if (_colliderChecker_tile.transform.position.y != tileObject.TopPointY())
            return false;

        return true;
	}

    TileObject TopPosTile()
    {
        Vector2 hitPoint = Vector2.zero;
        TileObject tileObject = _colliderChecker_tile.GetHit<TileObject>(
            BoxColliderChecker.CompareBase.MaxY, ref hitPoint);

        return tileObject;
    }
}
