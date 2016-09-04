using UnityEngine;
using System.Collections;

public abstract class ActionObject : MonoBehaviour
{
    [SerializeField]
    public Vector2 LookAt
    {
        get
        {
            if (transform.localScale.x < 0f)
                return Vector2.left;
            
            return Vector2.right;
        }
    }

    [SerializeField]
    protected Vector2 _baseScale = Vector2.one;
    public Vector2 BaseScale { get { return _baseScale; } }

    // Method

    #region MonoBehaviour event

    void Awake()
    {
        _baseScale = transform.localScale;
    }

    #endregion

    #region Position, Scale

    public void SetPos(Vector2 pos2D)
    {
        SetPos(new Vector3(pos2D.x, pos2D.y, transform.position.z));
    }

    public void SetPos(Vector3 pos3D)
    {
        transform.position = pos3D;
    }

    public void SetLocalPos(Vector2 pos2D)
    {
        transform.localPosition = pos2D;
    }

    public Vector2 GetPos()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    public Vector2 GetPos3D()
    {
        return transform.position;
    }

    public void Translate(Vector2 translateValue)
    {
        transform.Translate(translateValue);
    }

    public void TranslateAtSpaceWorld(Vector2 translateValue)
    {
        transform.Translate(translateValue, Space.World);
    }

    public void SetLookAt(Direction direction)
    {
        float localScaleX = Mathf.Abs(transform.localScale.x);

        switch (direction)
        {
            case Direction.Left:
                localScaleX *= -1f;
                break;

            case Direction.Right:
                break;

            default:
                return;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    public void SetLocalScale(Vector2 scaleValue )
    {
        transform.localScale = new Vector3(scaleValue.x, scaleValue.y, 1f);
    }

    #endregion

    #region Reset, Remove

    public virtual void Reset()
    {
    }

    public abstract void RemoveActionObject();

    #endregion

    #region Battle related

    public abstract bool IsEnableGetHit();
    public abstract void GetHit(BattleObject hitter);

    #endregion
}
