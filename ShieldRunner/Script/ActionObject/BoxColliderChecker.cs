using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxColliderCheckerResult
{
    public GameObject _target = null;
    public Vector2 _hitPoint = Vector2.zero;
}

[RequireComponent(typeof(BoxCollider2D))]
public class BoxColliderChecker : MonoBehaviour
{
    public enum CompareBase
    {
        MinX,
        MaxX,

        MinY,
        MaxY,
    }

    [SerializeField]
    LayerMask _checkLayer;

    [SerializeField]
    BoxCollider2D _boxCollider = null;

    // Method

    public Vector2 CheckerPos(CompareBase compareBase)
    {
        Vector2 pos = transform.position;

        switch (compareBase)
        {
            case CompareBase.MinX:
                pos.x -= _boxCollider.size.x * 0.5f * transform.lossyScale.x;
                break;

            case CompareBase.MaxX:
                pos.x += _boxCollider.size.x * 0.5f * transform.lossyScale.x;
                break;

            case CompareBase.MinY:
                pos.y -= _boxCollider.size.y * 0.5f * transform.lossyScale.y;
                break;

            case CompareBase.MaxY:
                pos.y += _boxCollider.size.y * 0.5f * transform.lossyScale.y;
                break;
        }

        return pos;
    }

    public RaycastHit2D[] GetHits()
    {
        Vector2 size = _boxCollider.size;
        Vector2 originPos = new Vector2(transform.position.x, transform.position.y) + _boxCollider.offset;

        return Physics2D.BoxCastAll(
            originPos, size, 0f, Vector2.zero, 0f, _checkLayer);
    }

    public List<T> GetHits<T>() where T : MonoBehaviour
    {
        List<T> hitList = new List<T>();

        RaycastHit2D[] hits = GetHits();

        for (int index = 0; index < hits.Length; ++index)
        {
            RaycastHit2D hit = hits[index];

            T tHit = hit.collider.GetComponent<T>();
            if (tHit == null)
                continue;

            hitList.Add(tHit);
        }
            
        return hitList;
    }

    public T GetHit<T>(CompareBase compareBase, ref Vector2 hitPoint) where T : MonoBehaviour
    {
        bool isLoopStart = true;
        GameObject hitGameObject = null;

        RaycastHit2D[] hits = GetHits();
        for (int index = 0; index < hits.Length; ++index)
        {
            RaycastHit2D eachHit = hits[index];

            Vector2 checkerPos = CheckerPos(compareBase);
            Vector2 eachBasePoint = BasePoint(compareBase, eachHit.collider.transform);

            if (CompareNewBasePoint(compareBase, checkerPos, eachBasePoint) == true)
                continue;

            if (isLoopStart == true)
            {
                hitGameObject = eachHit.collider.gameObject;;
                hitPoint = eachBasePoint;

                isLoopStart = false;
                continue;
            }
                
            if (CompareNewBasePoint(compareBase, hitPoint, eachBasePoint) == false)
                continue;

            hitGameObject = eachHit.collider.gameObject;;
            hitPoint = eachBasePoint;
        }

        if (hitGameObject == null)
            return null;

        return hitGameObject.GetComponent<T>();
    }

    Vector2 BasePoint(CompareBase compareBase, Transform trans)
    {
        Rect rect = GameObjectHelper.RectByTransform(trans);
        Vector2 pos = rect.center;

        switch (compareBase)
        {
            case CompareBase.MinX:
                pos.x = rect.xMin;
                break;

            case CompareBase.MaxX:
                pos.x = rect.xMax;
                break;

            case CompareBase.MinY:
                pos.y = rect.yMin;
                break;

            case CompareBase.MaxY:
                pos.y = rect.yMax;
                break;
        }

        return pos;
    }

    bool CompareNewBasePoint(CompareBase compareBase, Vector2 oldPoint, Vector2 newPoint)
    {
        switch (compareBase)
        {
            case CompareBase.MinX:
                return (newPoint.x < oldPoint.x);

            case CompareBase.MaxX:
                return (newPoint.x > oldPoint.x);

            case CompareBase.MinY:
                return (newPoint.y < oldPoint.y);

            case CompareBase.MaxY:
                return (newPoint.y > oldPoint.y);
        }

        return false;
    }
}
