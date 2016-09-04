using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class LineRendererObject : MonoBehaviour
{
	public const string DefaultMaterialPath = "Material/LineRenderer/";
	public const string DefaultMaterialFileName = "LineRendererDefaultMaterial";

	LineRenderer _lineRenderer = null;
	public LineRenderer LineRenderer
	{
		get
		{
			if (_lineRenderer == null)
			{
				_lineRenderer = GetComponent<LineRenderer>();
			}

			return _lineRenderer;
		}
	}

	// Method

	public static LineRendererObject Create()
	{
		return GameObjectHelper.NewGameObject<LineRendererObject>();
	}

	public void SetDefault()
	{
		LineRenderer.material = Resources.Load(
			DefaultMaterialPath + DefaultMaterialFileName) as Material;

		LineRenderer.receiveShadows = false;
		LineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

		LineRenderer.useLightProbes = false;
		LineRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

		LineRenderer.SetWidth(0.05f, 0.05f);
		LineRenderer.SetColors(Color.white, Color.white);
	}

	public void SetLineWidth(float width)
	{
		LineRenderer.SetWidth(width, width);
	}

	public void SetPosition(Vector3 startPos, Vector3 endPos)
	{
		LineRenderer.SetPosition(0, startPos);
		LineRenderer.SetPosition(1, endPos);
	}
}






