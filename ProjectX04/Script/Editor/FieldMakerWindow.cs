using UnityEngine;
using System.Collections;

#if UNITY_EDITOR

using UnityEditor;

public class FieldMakerWindow : EditorWindow
{
	const float DefaultSpaceValue = 10f;

	public static FieldMakerWindow _window = null;

	FieldMaker _fieldMaker = null;
	bool isDirtiedFieldMaker { get; set; }

	[MenuItem ("Custom Tool/Field maker window")]
	static void Init()
	{
		FieldMakerWindow._window = EditorWindow.GetWindow(typeof(FieldMakerWindow)) as FieldMakerWindow;
	}

	void Update()
	{
		if (isDirtiedFieldMaker == true)
		{
			Repaint();
			isDirtiedFieldMaker = false;
		}
	}

	void DirtiedFieldMaker()
	{
		isDirtiedFieldMaker = true;
	}
	
	void OnGUI()
	{
		Space();
		GUILayout.BeginVertical("Box");
		{
			GUILayout.Label("Field maker");
			Space();

			EditorGUILayout.ObjectField(
				_fieldMaker, typeof(FieldMaker), true, new GUILayoutOption[] {});

			if (GUILayout.Button("Find Field maker") == true)
			{
				FindFieldMakerInScene();

				if (_fieldMaker != null)
				{
					_fieldMaker._fieldTouchAction = null;
					_fieldMaker._fieldTouchAction += DirtiedFieldMaker;
				}
			}
		}
		GUILayout.EndVertical();

		if (_fieldMaker == null)
			return;

		Space();
		SetBrushButton();

		Space();
		switch (_fieldMaker._brushType)
		{
		case FieldMaker.BrushType.Select:	SetSelectedObjectGUI(); break;
		case FieldMaker.BrushType.Tile:		SetSelectTileGUI();		break;
		case FieldMaker.BrushType.Item:		SetSelectItemGUI();		break;
		case FieldMaker.BrushType.Potal:	SetSelectPortalGUI();	break;
		case FieldMaker.BrushType.Hider:	SetSelectHiderGUI();	break;
		default: break;
		}
		
		// Save / Load / Clear Button
		Space();
		OptionButton();
	}

	void Space()
	{
		Space(DefaultSpaceValue);
	}

	void Space(float value)
	{
		GUILayout.Space(value);
	}

	void FindFieldMakerInScene()
	{
		_fieldMaker = FindObjectOfType<FieldMaker>();
	}

	void SetBrushButton()
	{	
		GUILayout.BeginVertical("Box");
		{
			GUILayout.Label("Brush Type");

			Space();
			GUILayout.BeginHorizontal();
			{
				FieldMaker.BrushType brush = (FieldMaker.BrushType)EditorGUILayout.EnumPopup(_fieldMaker._brushType);
				_fieldMaker.SetSelectType(brush);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}

	void SetSelectedObjectGUI()
	{
		EditorGUILayout.BeginVertical("Box");
		{
			GUILayout.Label("Selected Object");
			
			Space();
			foreach (GameObject obj in _fieldMaker._selectedObjectList)
			{
				EditorGUILayout.ObjectField(obj.name, obj, obj.GetType(), true, new GUILayoutOption[] {});
			}
		}
		EditorGUILayout.EndVertical();
	}
	
	void SetSelectTileGUI()
	{
		EditorGUILayout.BeginVertical("Box");
		{
			GUILayout.Label("Select tile");

			Space();
			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("<<") == true)
				{
					_fieldMaker._tileSliderValue = Mathf.Max(0, _fieldMaker._tileSliderValue - 1);
				}
				
				int tileCount = _fieldMaker._tileList.Count;
				_fieldMaker._tileSliderValue = (int)EditorGUILayout.Slider(_fieldMaker._tileSliderValue, 0f, tileCount);
				
				if (GUILayout.Button(">>") == true)
				{
					_fieldMaker._tileSliderValue = Mathf.Min(tileCount, _fieldMaker._tileSliderValue + 1);
				}
			}
			EditorGUILayout.EndHorizontal();
			
			Space();
			if (_fieldMaker._tileSliderValue == 0)
			{
				_fieldMaker._selectTile = null;
			}
			else
			{
				_fieldMaker._selectTile = _fieldMaker._tileList[_fieldMaker._tileSliderValue - 1];
			}
			
			if (_fieldMaker._selectTile)
			{
				SpriteRenderer renderer = _fieldMaker._selectTile.GetComponent<SpriteRenderer>();
				if (renderer)
				{
					if (renderer.sprite && renderer.sprite.texture)
					{
						GUILayout.Box(renderer.sprite.texture);
					}
				}
				
				GUILayout.Label(string.Format("{0}", _fieldMaker._selectTile.name), EditorStyles.boldLabel);
			}
		}
		EditorGUILayout.EndVertical();
	}
	
	void SetSelectItemGUI()
	{
		EditorGUILayout.BeginVertical("Box");
		{
			GUILayout.Label("Select Item");
			
			Space();
			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("<<") == true)
				{
					_fieldMaker._itemSliderValue = Mathf.Max(0, _fieldMaker._itemSliderValue - 1);
				}
				
				int itemCount = _fieldMaker._itemList.Count;
				_fieldMaker._itemSliderValue = (int)EditorGUILayout.Slider(_fieldMaker._itemSliderValue, 0f, itemCount);
				
				if (GUILayout.Button(">>") == true)
				{
					_fieldMaker._itemSliderValue = Mathf.Min(itemCount, _fieldMaker._itemSliderValue + 1);
				}
			}
			EditorGUILayout.EndHorizontal();
			
			Space();
			if (_fieldMaker._itemSliderValue == 0)
			{
				_fieldMaker._selectItem = null;
			}
			else
			{
				_fieldMaker._selectItem = _fieldMaker._itemList[_fieldMaker._itemSliderValue - 1];
			}
			
			if (_fieldMaker._selectItem)
			{
				SpriteRenderer renderer = _fieldMaker._selectItem.GetComponent<SpriteRenderer>();
				if (renderer)
				{
					if (renderer.sprite && renderer.sprite.texture)
					{
						GUILayout.Box(renderer.sprite.texture);
					}
				}
				
				GUILayout.Label(string.Format("{0}", _fieldMaker._selectItem.name), EditorStyles.boldLabel);
			}
		}
		EditorGUILayout.EndVertical();
	}
	
	void SetSelectPortalGUI()
	{
		EditorGUILayout.BeginVertical("Box");
		{
			GUILayout.Label("Select Portal");
			
			Space();
			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("<<") == true)
				{
					_fieldMaker._portalSliderValue = Mathf.Max(0, _fieldMaker._portalSliderValue - 1);
				}
				
				int portalCount = _fieldMaker._portalList.Count;
				_fieldMaker._portalSliderValue = (int)EditorGUILayout.Slider(_fieldMaker._portalSliderValue, 0f, portalCount);
				
				if (GUILayout.Button(">>") == true)
				{
					_fieldMaker._portalSliderValue = Mathf.Min(portalCount, _fieldMaker._portalSliderValue + 1);
				}
			}
			EditorGUILayout.EndHorizontal();
			
			Space();
			if (_fieldMaker._portalSliderValue == 0)
			{
				_fieldMaker._selectPortal = null;
			}
			else
			{
				_fieldMaker._selectPortal = _fieldMaker._portalList[_fieldMaker._portalSliderValue - 1];
			}
			
			if (_fieldMaker._selectPortal)
			{
				SpriteRenderer renderer = _fieldMaker._selectPortal.GetComponent<SpriteRenderer>();
				if (renderer)
				{
					if (renderer.sprite && renderer.sprite.texture)
					{
						GUILayout.Box(renderer.sprite.texture);
					}
				}
				
				GUILayout.Label(string.Format("{0}", _fieldMaker._selectPortal.name), EditorStyles.boldLabel);
			}
		}
		EditorGUILayout.EndVertical();
	}
	
	void SetSelectHiderGUI()
	{
		EditorGUILayout.BeginVertical("Box");
		{
			GUILayout.Label("Select Hider");
			
			Space();
			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("<<") == true)
				{
					_fieldMaker._hiderSliderValue = Mathf.Max(0, _fieldMaker._hiderSliderValue - 1);
				}
				
				int hiderCount = _fieldMaker._hiderList.Count;
				_fieldMaker._hiderSliderValue = (int)EditorGUILayout.Slider(_fieldMaker._hiderSliderValue, 0f, hiderCount);
				
				if (GUILayout.Button(">>") == true)
				{
					_fieldMaker._hiderSliderValue = Mathf.Min(hiderCount, _fieldMaker._hiderSliderValue + 1);
				}
			}
			EditorGUILayout.EndHorizontal();
			
			Space();
			if (_fieldMaker._hiderSliderValue == 0)
			{
				_fieldMaker._selectHider = null;
			}
			else
			{
				_fieldMaker._selectHider = _fieldMaker._hiderList[_fieldMaker._hiderSliderValue - 1];
			}
			
			if (_fieldMaker._selectHider)
			{
				SpriteRenderer renderer = _fieldMaker._selectHider.GetComponent<SpriteRenderer>();
				if (renderer)
				{
					if (renderer.sprite && renderer.sprite.texture)
					{
						GUILayout.Box(renderer.sprite.texture);
					}
				}
				
				GUILayout.Label(string.Format("{0}", _fieldMaker._selectHider.name), EditorStyles.boldLabel);
			}
		}
		EditorGUILayout.EndVertical();
	}
	
	void OptionButton()
	{
		EditorGUILayout.BeginVertical("Box");
		{
			GUILayout.Label("Option button");

			Space();
			_fieldMaker._saveLoadFieldIndex = EditorGUILayout.IntField("Field Index : ", _fieldMaker._saveLoadFieldIndex);
			
			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button ("Save") == true) {
					//					_fieldMaker.ShowSavePopup ();
					_fieldMaker.SaveField(_fieldMaker._saveLoadFieldIndex);
				}
				
				if (GUILayout.Button ("Load") == true) {
					//_fieldMaker.ShowLoadPopup ();
					_fieldMaker.LoadField(_fieldMaker._saveLoadFieldIndex);
				}
				
				if (GUILayout.Button ("Clear") == true) {
					_fieldMaker.ClearField();
				}
			}
			EditorGUILayout.EndHorizontal();
			Space();
		}
		EditorGUILayout.EndVertical();
	}
}

#endif
