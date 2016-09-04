using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using System.Collections;
using System.Collections.Generic;

using CustomXmlSerializerUtil;

public class XmlSerializeWindow : EditorWindow {

	// Property

	public UnityEngine.Object obj = null;
	public SerializedProperty property;

	// Mathod

	[MenuItem ("Custom Tool/Xml serialize window")]

	static void Init()
	{
		EditorWindow.GetWindow(typeof(XmlSerializeWindow));
	}
//
//	[Serializable]
//	public class ButtonDoubleClickedEvent : UnityEvent { }
//	
//	// Event delegates triggered on Doubleclick.
//	[FormerlySerializedAs("onDoubleClick")]
//	[SerializeField]
//	private ButtonDoubleClickedEvent m_OnDoubleClick = new ButtonDoubleClickedEvent();
//
//	public ButtonDoubleClickedEvent onDoubleClick
//	{
//		get { return m_OnDoubleClick; }
//		set { m_OnDoubleClick = value; }
//	}
//
//	SerializedProperty m_OnDoubleClickProperty;
	
	void OnGUI()
	{
//		m_OnDoubleClickProperty = serializedObject.FindProperty("m_OnDoubleClick");

		this.maxSize = new Vector2(300f, 300f);

		GUILayout.Label("Base settings", EditorStyles.boldLabel);

		EditorGUILayout.Space();

		EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(300f));
		{
			EditorGUILayout.Space();

			obj =  EditorGUILayout.ObjectField("Serialize Target:", obj, typeof(UnityEngine.Object), false);
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("New empty xml data.") == true)
				{
					CustomXmlSerializerOld.instance.SaveEmptyData(obj);
				}

				if (GUILayout.Button("Load xml data.") == true)
				{
		//			CustomXmlSerializerOld.instance.LoadData(obj);
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}
		EditorGUILayout.EndVertical();
	}
}
