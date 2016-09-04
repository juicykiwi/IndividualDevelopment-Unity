using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatCharacter : MonoBehaviour {

	//public GameObject[] _teamA;
	UIUnitSlot teamA ;		//인트로 UI에서 선택된 유닛
	//public GameObject[] _teamB01;
	//public GameObject[] _teamB02;
	//public GameObject[] _teamB03;

	public Transform _teamAposition01;
	public Transform _teamAposition02;
	//public Transform _teamBposition01;
	//public Transform _teamBposition02;
	//public Transform _teamBposition03;

	public Transform _teamBpositions;
	public GameObject[] _teamBObj;
	public float[] _spawnOrder;
	public int[] _spawnPosIndex;


	public Transform _cameraTarget = null;
	public Transform _leaderPos = null;

	public GameObject _HpBack;
	public GameObject _summonerAura;
	public GameObject _teamAcircle;
	public GameObject _teamBcircle;

	public Transform _cameraPos;
	public float _crtCenterX = 0f;
	public float _crtCenterZ = 0f;
	protected float _xSum = 0f;
	protected float _zSum = 0f;

	public GameObject _alertArrow;
	public Transform _alertPanel;
	protected Transform[] _alertPos = new Transform[20];


	protected Transform[] _unitPos = new Transform[5];

	protected int spawnIndex = 0;
	protected float spawnTime = Time.time;

	void Awake()
	{
		//a
	}

	// Use this for initialization
	void Start () {


		UIUnitSlot teamA = (UIUnitSlot)FindObjectOfType<UIUnitSlot>();
		CharacterManager characterManager = (CharacterManager)FindObjectOfType<CharacterManager>();
		UIUnitSlot summonerPick = (UIUnitSlot)FindObjectOfType<UIUnitSlot> ();

		if (characterManager == null)
			return;
		
		for (int i = 0; i < 5; i++) {
			//A team 유닛생성
			GameObject aa = Instantiate (teamA._unitList[teamA._startUnitNum[i]]) as GameObject;
			//GameObject aa = Instantiate (_teamA [i]) as GameObject;    		
			switch (summonerPick._startUnitNum[0]) {
			case 0: aa.transform.parent = _teamAposition01.GetChild(i).transform;
				_teamAposition02.gameObject.SetActive(false);
				break;
			case 1: aa.transform.parent = _teamAposition02.GetChild(i).transform;
				_teamAposition01.gameObject.SetActive(false);
				break;
			default:
				break;
			}
			aa.transform.localPosition = Vector3.zero;
			aa.transform.localRotation = Quaternion.identity;
			aa.transform.parent.FindChild("Dummy").gameObject.SetActive(false);

			_unitPos[i] = aa.transform as Transform;

			//A team 진영/유닛타입 설정
			BaseCharacter baseCharacter_A = aa.GetComponent<BaseCharacter>();   
			baseCharacter_A._battleSide = BattleSide.A;
			baseCharacter_A._unitIndex = i;
			characterManager._characterList.Add(baseCharacter_A);
			if (baseCharacter_A._mainType == CharacterMainType.Summoner)
			{
				characterManager._playerSummoner = baseCharacter_A;
				aa.tag = "Summoner";
				GameObject summonerAura = Instantiate (_summonerAura) as GameObject; //오오라 붙이기
				summonerAura.transform.parent = aa.transform;
				summonerAura.transform.localPosition = Vector3.zero;
				_leaderPos.transform.position = aa.transform.localPosition;
			}
			GameObject teamAcircle = Instantiate (_teamAcircle) as GameObject;
			teamAcircle.transform.parent = aa.transform;
			teamAcircle.transform.localPosition = Vector3.zero;

			//A team HP바 배경생성
			GameObject hpbackA = Instantiate (_HpBack) as GameObject;			
			hpbackA.transform.parent = aa.transform;
			hpbackA.transform.localPosition = new Vector3(0.0f,_HpBack.transform.localPosition.y,0.0f);
		}
		//A team 초기좌표 설정
		for (int i = 0; i < 5; i++) {
			_xSum += (float)_unitPos[i].position.x;
			_zSum += (float)_unitPos[i].position.z;
				}
		_crtCenterX = _xSum * 0.2f ;
		_crtCenterZ = _zSum * 0.2f ;


		/*
		for (int i = 0; i < 5; i++) {
			//B team01 유닛생성
			GameObject bb = Instantiate (_teamB01 [i]) as GameObject;
			bb.transform.parent = _teamBposition01.GetChild(i).transform;
			bb.transform.localPosition = Vector3.zero;
			bb.transform.localRotation = Quaternion.identity;
			bb.transform.parent.FindChild("Dummy").gameObject.SetActive(false);
			
			//B team 진영/유닛타입 설정
			BaseCharacter baseCharacter_B = bb.GetComponent<BaseCharacter>();
			baseCharacter_B._battleSide = BattleSide.B;
			characterManager._characterList.Add(baseCharacter_B);
			if (baseCharacter_B._mainType == CharacterMainType.Summoner)
				characterManager._playerSummoner = baseCharacter_B;
			
			//B team HP바 배경생성
			GameObject hpbackB = Instantiate (_HpBack) as GameObject;
			hpbackB.transform.parent = bb.transform;
			hpbackB.transform.localPosition = new Vector3(0.0f,_HpBack.transform.localPosition.y,0.0f);

			GameObject teamBcircle = Instantiate (_teamBcircle) as GameObject;
			teamBcircle.transform.parent = bb.transform;
			teamBcircle.transform.localPosition = Vector3.zero;
			}

		for (int i = 0; i < 5; i++) {
			//B team02 유닛생성
			GameObject bb = Instantiate (_teamB02 [i]) as GameObject;
			bb.transform.parent = _teamBposition02.GetChild(i).transform;
			bb.transform.localPosition = Vector3.zero;
			bb.transform.localRotation = Quaternion.identity;
			bb.transform.parent.FindChild("Dummy").gameObject.SetActive(false);
			
			//B team 진영/유닛타입 설정
			BaseCharacter baseCharacter_B = bb.GetComponent<BaseCharacter>();
			baseCharacter_B._battleSide = BattleSide.B;
			characterManager._characterList.Add(baseCharacter_B);
			if (baseCharacter_B._mainType == CharacterMainType.Summoner)
				characterManager._playerSummoner = baseCharacter_B;
			
			//B team HP바 배경생성
			GameObject hpbackB = Instantiate (_HpBack) as GameObject;
			hpbackB.transform.parent = bb.transform;
			hpbackB.transform.localPosition = new Vector3(0.0f,_HpBack.transform.localPosition.y,0.0f);
			_teamBposition02.transform.gameObject.SetActive(false);

			GameObject teamBcircle = Instantiate (_teamBcircle) as GameObject;
			teamBcircle.transform.parent = bb.transform;
			teamBcircle.transform.localPosition = Vector3.zero;
		}

		for (int i = 0; i < 5; i++) {
			//B team03 유닛생성
			GameObject bb = Instantiate (_teamB03 [i]) as GameObject;
			bb.transform.parent = _teamBposition03.GetChild(i).transform;
			bb.transform.localPosition = Vector3.zero;
			bb.transform.localRotation = Quaternion.identity;
			bb.transform.parent.FindChild("Dummy").gameObject.SetActive(false);
			
			//B team 진영/유닛타입 설정
			BaseCharacter baseCharacter_B = bb.GetComponent<BaseCharacter>();
			baseCharacter_B._battleSide = BattleSide.B;
			characterManager._characterList.Add(baseCharacter_B);
			if (baseCharacter_B._mainType == CharacterMainType.Summoner)
				characterManager._playerSummoner = baseCharacter_B;
			
			//B team HP바 배경생성
			GameObject hpbackB = Instantiate (_HpBack) as GameObject;
			hpbackB.transform.parent = bb.transform;
			hpbackB.transform.localPosition = new Vector3(0.0f,_HpBack.transform.localPosition.y,0.0f);
			_teamBposition03.transform.gameObject.SetActive(false);

			GameObject teamBcircle = Instantiate (_teamBcircle) as GameObject;
			teamBcircle.transform.parent = bb.transform;
			teamBcircle.transform.localPosition = Vector3.zero;
		}
*/

		CharacterManager.GetInstance().UpdateFormationPosList();
	}
		
	// Update is called once per frame
	void Update () {
		CharacterManager characterManager = (CharacterManager)FindObjectOfType<CharacterManager>();
		List<BaseCharacter> enemyList = CharacterManager.GetInstance().EqualBattleSideCharacters(BattleSide.B);


		/*
		if (enemyList.Count <= 10)
		{
			_teamBposition02.gameObject.SetActive(true);
			//Debug.Log("2stage!!");
		}

		if (enemyList.Count <= 5)
		{
			_teamBposition03.gameObject.SetActive(true);
			//Debug.Log("3stage!!");
		}
*/
		if ( _spawnOrder.Length - spawnIndex > 0 ) {
			if (Time.time - spawnTime >= _spawnOrder[spawnIndex] || enemyList.Count <= 0) {
				
				GameObject bb = Instantiate (_teamBObj[spawnIndex]) as GameObject;
				bb.transform.parent = _teamBpositions.GetChild(_spawnPosIndex[spawnIndex]).transform;
				bb.transform.localPosition = Vector3.zero;
				bb.transform.localRotation = Quaternion.identity;
				bb.transform.parent.FindChild("Dummy").gameObject.SetActive(false);
				
				BaseCharacter baseCharacter_B = bb.GetComponent<BaseCharacter>();
				baseCharacter_B._battleSide = BattleSide.B;
				characterManager._characterList.Add(baseCharacter_B);
				if (baseCharacter_B._mainType == CharacterMainType.Summoner)
					characterManager._playerSummoner = baseCharacter_B;

				GameObject hpbackB = Instantiate (_HpBack) as GameObject;
				hpbackB.transform.parent = bb.transform;
				hpbackB.transform.localPosition = new Vector3(0.0f,_HpBack.transform.localPosition.y,0.0f);
				GameObject teamBcircle = Instantiate (_teamBcircle) as GameObject;
				teamBcircle.transform.parent = bb.transform;
				teamBcircle.transform.localPosition = Vector3.zero;

				//GameObject alertArrow = Instantiate (_alertArrow) as GameObject;

				
				spawnIndex += 1;
				spawnTime = Time.time;			
			}
		}


		_xSum = 0f;
		_zSum = 0f;

		List<BaseCharacter> allyList = CharacterManager.GetInstance().EqualBattleSideCharacters(BattleSide.A);
		if (allyList.Count > 0)
		{
			foreach (BaseCharacter allyCharacter in allyList)
			{
				_xSum  += (float)allyCharacter.transform.position.x;
				_zSum  += (float)allyCharacter.transform.position.z;
			}

			_cameraPos.transform.position += new Vector3 (_xSum / allyList.Count - _crtCenterX , 0f, _zSum / allyList.Count - _crtCenterZ );
			_crtCenterX = _xSum / allyList.Count;
			_crtCenterZ = _zSum / allyList.Count;
		}
	}



}