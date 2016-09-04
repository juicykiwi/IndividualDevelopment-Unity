using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIScrollView : MonoBehaviour
{
    [SerializeField]
    protected ScrollRect _scrollRect = null;

    [SerializeField]
    protected GridLayoutGroup _gridLayoutGroup = null;

    [SerializeField]
    protected GameObject _contentRoot = null;

    [SerializeField]
    protected UIScrollSlot _template = null;

    [SerializeField]
    protected Vector2 _size = Vector2.zero;

    [SerializeField]
    protected List<UIScrollSlot> _contentList = new List<UIScrollSlot>();


    protected virtual void Awake()
    {
        _template.gameObject.SetActive(false);
        _size = _scrollRect.content.sizeDelta;
    }

    public UIScrollSlot CreateScrollSlot()
    {
        return Instantiate<UIScrollSlot>(_template);
    }

    public void Clear()
    {
        for (int index = 0; index < _contentList.Count; ++index)
        {
            if (_contentList[index] == null)
                continue;

            if (Application.isPlaying == true)
            {
                Destroy(_contentList[index].gameObject);
            }
            else
            {
                DestroyImmediate(_contentList[index].gameObject);
            }
        }

        _contentList.Clear();
    }

    public void AddContent(UIScrollSlot content)
    {
        if (content == null)
            return;
        
        content.transform.SetParent(_contentRoot.transform);
        content.gameObject.SetActive(true);

        content.transform.localPosition = Vector3.zero;
        content.transform.localRotation = Quaternion.identity;
        content.transform.localScale = _template.transform.localScale;

        _contentList.Add(content);

        _size.x = ScrollRectSizeX();
        _scrollRect.content.sizeDelta = _size;
    }

    public void RemoveContent()
    {
        int lastIndex = _contentList.Count - 1;
        if (lastIndex < 0)
            return;

        if (Application.isPlaying == true)
        {
            Destroy(_contentList[lastIndex].gameObject);
        }
        else
        {
            DestroyImmediate(_contentList[lastIndex].gameObject);
        }
        _contentList.RemoveAt(lastIndex);

        _size.x = ScrollRectSizeX();
        _scrollRect.content.sizeDelta = _size;
    }

    public float ScrollRectSizeX()
    {
        float gridWidth = (_contentList.Count * _gridLayoutGroup.cellSize.x) +
            (_gridLayoutGroup.spacing.x * Mathf.Max(0, _contentList.Count - 1)) -
            (_scrollRect.GetComponent<RectTransform>().sizeDelta.x);

        return Mathf.Max(0f, gridWidth);
    }
}


#if UNITY_EDITOR

[CanEditMultipleObjects]
[CustomEditor(typeof(UIScrollView))]
public class UIScrollViewEditor<T> : Editor where T : MonoBehaviour
{
    UIScrollView _target = null;

    public override void OnInspectorGUI()
    {
        _target = target as UIScrollView;

        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Add content") == true)
            {
                UIScrollSlot scrollSlot = _target.CreateScrollSlot();    
                _target.AddContent(scrollSlot);
            }
            else if (GUILayout.Button("Remove content") == true)
            {
                _target.RemoveContent();
            }
            else if (GUILayout.Button("Clear content") == true)
            {
                _target.Clear();
            }
        }
        GUILayout.EndHorizontal();
    }
}

#endif