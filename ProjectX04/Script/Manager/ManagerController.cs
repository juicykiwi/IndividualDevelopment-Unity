using UnityEngine;
using System.Collections;

public class ManagerController : Singleton<ManagerController>
{
    GameObject _dataManagerObject = null;
    public GameObject DataManagerObject { get { return _dataManagerObject; } }

    void Awake()
    {
        _dataManagerObject = new GameObject("DataManager");
        _dataManagerObject.transform.SetParent(transform);
    }
}
