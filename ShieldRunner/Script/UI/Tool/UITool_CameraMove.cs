using UnityEngine;
using System.Collections;

public class UITool_CameraMove : MonoBehaviour
{
	public void OnCameraMoveButton_X(float x)
	{
		Transform cameraTrans = CameraManager.instance.MainCamera.transform;
		cameraTrans.Translate(new Vector2(x, 0f));
	}

	public void OnCameraMoveButton_Y(float y)
	{
		Transform cameraTrans = CameraManager.instance.MainCamera.transform;
		cameraTrans.Translate(new Vector2(0f, y));
	}
}
