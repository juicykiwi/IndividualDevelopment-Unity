using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIAlphaMask : MonoBehaviour {

	// Field & Property

	Image _maskImage = null;
	Material _maskMaterial = null;

	// Method

	void Awake () {
		_maskImage = this.GetComponent<Image>();

	}

	// Use this for initialization
	void Start () {
		_maskMaterial = _maskImage.material;

		if (EventManager.instance != null)
		{

		}
	}

	void OnDestroy () {
		if (EventManager.instance != null)
		{

		}
	}

	void OnEnable () {

	}

	void OnDisable () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DelChaMove(ChaController cha, List<TileController> tileControllerList)
	{
		if (cha == null)
			return;

		if (cha.team != Team.UserTeam)
			return;

		UpdateMask(cha);
	}

	public void UpdateMask(ChaController cha)
	{
		if (cha == null)
			return;

		Vector3 screenPos = CameraManager.instance._main.WorldToScreenPoint(cha.transform.position);
		Vector4 maskPos = new Vector4(screenPos.x, screenPos.y, screenPos.z);
		_maskMaterial.SetVector("_CutScreenPos", maskPos);
	}
}
