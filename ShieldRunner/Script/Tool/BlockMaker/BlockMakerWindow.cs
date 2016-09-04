using UnityEngine;
using System.Collections;

#if UNITY_EDITOR

using UnityEditor;

public class BlockMakerWindow : EditorWindow
{
    static BlockMakerWindow _instance = null;

    static public BlockMakerWindow instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = EditorWindow.GetWindow<BlockMakerWindow>(
                    typeof(BlockMakerWindow).Name, false);
            }

            return _instance;
        }
    }

    [SerializeField]
    int _elementId = 0;
    public int ElementId
    { 
        get { return _elementId; }
        set { _elementId = value; }
    }

    public float _blockId = 0f;

	// Method

	[MenuItem("Tool/BlockMakerWindow")]
	static public void Init()
	{
		BlockMakerWindow window = BlockMakerWindow.instance;
        if (window != null)
        {
//            Vector2 windowSize = new Vector2(200, 200);
//            Rect dropDownButtonRect = new Rect(100, 100, 200, 200);
//            window.ShowAsDropDown(dropDownButtonRect, windowSize);
            window.Focus();
            window.ResetData();
        }
	}

	public void ResetData()
	{
        _blockId = 0f;
        ElementId = 0;
	}

	void OnGUI()
	{
		EditorGUILayout.Space();
		GUILayout.Label("Block Maker Window");
		EditorGUILayout.Space();

		ShowSelectBlockIdField();
		EditorGUILayout.Space();

		ShowLoadSaveClearButton();
		EditorGUILayout.Space();

		BrushControl();
		EditorGUILayout.Space();

        ShowSelectElementIdInt();
        EditorGUILayout.Space();
	}

	void ShowSelectBlockIdField()
	{
		EditorGUILayout.BeginHorizontal();
		{
			GUILayout.Label ("Block ID : ");
            _blockId = EditorGUILayout.FloatField(_blockId);
		}
		EditorGUILayout.EndHorizontal();
	}

    void ShowSelectElementIdInt()
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label ("Element ID : ");
            _elementId = EditorGUILayout.IntField(_elementId);
        }
        EditorGUILayout.EndHorizontal();
    }

	void ShowLoadSaveClearButton()
	{
		EditorGUILayout.BeginHorizontal();
		{
			if (GUILayout.Button("Load") == true)
			{
				OnLoadButton(_blockId);
			}

			if (GUILayout.Button("Save") == true)
			{
				OnSaveButton(_blockId);
			}

			if (GUILayout.Button("Clear") == true)
			{
				OnClearButton();
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	// Brush

	void BrushControl()
	{
		GUILayout.Label("Brush control");
		ShowSelectBrushButton();
	}

	void ShowSelectBrushButton()
	{
		if (GUILayout.Button("Tile") == true)
		{
            BlockMakerSceneControl.instance.Brush = new BlockToolTileBrush();
		}

		else if (GUILayout.Button("Monster") == true)
		{
            BlockMakerSceneControl.instance.Brush = new BlockToolMonsterBrush();
		}

        else if (GUILayout.Button("PickupItem") == true)
        {
            BlockMakerSceneControl.instance.Brush = new BlockToolPickupItemBrush();
        }

		else if (GUILayout.Button("Eraser") == true)
		{
            BlockMakerSceneControl.instance.Brush = new BlockToolEraserBrush();
		}
	}

    // Button event

	void OnLoadButton(float blockId)
	{
		BlockMakerSceneControl.instance.LoadBlockObject(blockId);
	}

	void OnSaveButton(float blockId)
	{
		BlockMakerSceneControl.instance.SaveBlockObject(blockId);
	}

	void OnClearButton()
	{
		BlockMakerSceneControl.instance.ClearBlockObject();
        MonsterManager.instance.Clear();
	}
}

#endif