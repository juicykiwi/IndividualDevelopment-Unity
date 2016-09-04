using UnityEngine;
using System.Collections;

public class UIFormationButton : MonoBehaviour {

	public float _formationAngle = 0.0f;

	void Awake () {

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick()
	{
//		CharacterManager.GetInstance().ChangeFormationForCameraCenter(_formationAngle);
//		CharacterManager.GetInstance().ChangeFormationForCharPosAverage(_formationAngle);

		if (CharacterManager.GetInstance()._playerSummoner == null)
		{
			CharacterManager.GetInstance().ChangeFormationForCharPosAverage(_formationAngle);
		}
		else
		{
			CharacterManager.GetInstance().ChangeFormationForSummonerPos(_formationAngle);
		}
	}
}
