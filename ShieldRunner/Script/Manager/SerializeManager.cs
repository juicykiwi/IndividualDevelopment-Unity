using UnityEngine;
using System.Collections;
using CustomXmlSerializerUtil;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SerializeManager : Singleton<SerializeManager>, ITitleLoadAsynk
{
	[SerializeField]
	Object _serializeObject = null;
	public Object SerializeObject { get { return _serializeObject; } }

	public string _saveEmptyPath = "Data/EmptyData/";

    // Method

    #region ITitleLoadAsynk

    public void InitAsynk()
    {
        CreateInstance();
    }

    #endregion
}

#if UNITY_EDITOR

[CustomEditor(typeof(SerializeManager))]
public class SerializeManagerEditor : Editor
{
	SerializeManager _target = null;

    // Method

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI();

		_target = (SerializeManager)target;

		GUILayout.Space(20f);

		if (GUILayout.Button("Create empty data") == true)
		{
			if (_target.SerializeObject == null)
				return;

			CustomXmlSerializerOld.instance.SaveEmptyData(_target.SerializeObject, _target._saveEmptyPath);

			AssetDatabase.Refresh();
		}
	}
}

#endif
