using UnityEngine;
using System.Collections;
using System.IO;

#if UNITY_STANDALONE
using UnityEditor;
#endif

[RequireComponent(typeof(SpriteRenderer))]

public class TextureMixer : MonoBehaviour {

	public const string MixPath = "/Resources/TextureMixer";
	public const string MixFileName = "/MixTexture.png";

	public const string ShaderBlendType = "Custom/TextureMixerBlend";
	public const string ShaderAddType = "Custom/TextureMixerAdd";

	public new Renderer renderer
	{
		get
		{
			return this.GetComponent<Renderer>();
		}
	}

	public Sprite GetSprite
	{
		get
		{
			return this.GetComponent<SpriteRenderer>().sprite;
		}
	}

	// Method

	public void CreateToFile(Texture2D texture)
	{
		if (texture == null)
		{
			Debug.Log("Texture is null.");
			return;
		}

		if (Directory.Exists(Application.dataPath + TextureMixer.MixPath) == false)
		{
			Directory.CreateDirectory(Application.dataPath + TextureMixer.MixPath);
		}
		
		string path = Application.dataPath + TextureMixer.MixPath + TextureMixer.MixFileName;
		FileStream stream = new FileStream(path, FileMode.Create);
		
		byte[] encodeByte = texture.EncodeToPNG();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write(encodeByte);
		stream.Close();
	}
}

#if UNITY_STANDALONE

[CanEditMultipleObjects]
[CustomEditor(typeof(TextureMixer))]

public class TextureBlenderEditor : Editor {
	
	TextureMixer _target = null;
	
	// Method

	public override bool RequiresConstantRepaint()
	{
		return true;
	}
	
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();
		
		_target = this.target as TextureMixer;
		
		EditorGUILayout.HelpBox(
			"You Setting Texture Option\n" +
			"1. TextureType : Advance\n" +
			"2. Read/Write Enabled : Check",
			MessageType.Info);

		if (GUILayout.Button("Blend") == true)
		{
			Debug.Log("Click Blend button.");

			_target.renderer.material = new Material(Shader.Find(TextureMixer.ShaderBlendType));
		}

		if (GUILayout.Button("Add") == true)
		{
			Debug.Log("Click Add button.");

			_target.renderer.material = new Material(Shader.Find(TextureMixer.ShaderAddType));
		}

		if (GUILayout.Button("Save") == true)
		{
			Debug.Log("Click Save button.");

//			Material mat = _target.renderer.sharedMaterial;
//			Texture2D texture = (mat.mainTexture as Texture2D);

			Texture2D texture = _target.GetSprite.texture;
			_target.CreateToFile(texture);
		}
	}
}

#endif
