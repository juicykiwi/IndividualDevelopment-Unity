using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public abstract class BlockElementControl<TInfo, TObject> where TObject : MonoBehaviour
{
    [SerializeField]
    protected List<TObject> _objectList = new List<TObject>();
    public List<TObject> ObjectList { get { return _objectList; } }

    protected BlockObject _parentBlockObject = null;

    protected IEnumerator<TInfo> _createEnumerator = null;
    public IEnumerator<TInfo> CreateEnumerator { set { _createEnumerator = value; } }

    public bool _isEnded = false;
    public bool IsEnded { get { return _isEnded; } }

    // abstract

    protected abstract TObject CreateElement(TInfo creationInfo);

    public abstract void Add(TObject tObject);
    public abstract void Remove(TObject tObject);

    // Method

    public void Init(BlockObject parentBlockObject)
    {
        _parentBlockObject = parentBlockObject;
    }

    public List<TObject> Create(bool isLimit, int count = 1)
    {
        List<TObject> createdList = new List<TObject>();

        if (_createEnumerator == null)
            return createdList;

        do
        {
            if (_createEnumerator.MoveNext() == false)
            {
                _isEnded = true;
                break;
            }

            if (_createEnumerator.Current == null)
                continue;

            TObject tObject = CreateElement(_createEnumerator.Current);
            if (tObject == null)
                continue;

            _objectList.Add(tObject);

            if (isLimit == false)
                continue;

            if (--count <= 0)
                break;
            
        } while (true);

        return createdList;
    }

    public void Sort()
    {
        _objectList.Sort(new BlockElementComparer<TObject>());
    }
}

/// <summary>
/// Block element comparer.
/// x 좌표 위치로 비교
/// </summary>
public class BlockElementComparer<TObject> : IComparer<TObject> where TObject : MonoBehaviour
{
    public int Compare(TObject x, TObject y)
    {
        if (x.transform.position.x < y.transform.position.x)
            return -1;

        if (x.transform.position.x > y.transform.position.x)
            return 1;

        return 0;
    }
}
