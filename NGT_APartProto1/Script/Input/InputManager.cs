using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	protected static InputManager _instance = null;
	public static InputManager GetInstance() { return _instance; }

	protected BaseCharacter _touchedCharacter = null;
	protected Vector3 _touchFieldPoint = Vector3.zero;

	protected TouchPhase _mousePhase = TouchPhase.Canceled;
	protected Vector3 _mouseBeganPosition = new Vector3();

	void Awake () {
		if (_instance == null)
		{
			_instance = this;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

#if UNITY_EDITOR
		switch (TouchStateFromMouse())
#elif UNITY_ANDROID
		if (Input.touchCount <= 0)
			return;
		
		switch (Input.GetTouch(0).phase)
#endif
		{
		case TouchPhase.Began:
		{
//			Debug.Log(string.Format("mousePoint : {0} / {1} / {2}", Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));

			BaseCharacter character = HitCharacerFromMouse();
			if (character)
			{
				if (character._battleSide != BattleSide.A)
					break;

				_touchedCharacter = character;
			}
			else
			{
				Vector3 hitFieldPos = new Vector3();
				if (HitFieldPosition(ref hitFieldPos) == true)
				{
					_touchFieldPoint = hitFieldPos;
				}
			}
		}
			break;

		case TouchPhase.Ended:
		{
			if (_touchedCharacter)
			{
				Vector3 hitFieldPos = new Vector3();
				if (HitFieldPosition(ref hitFieldPos) == true)
				{
//					Debug.Log(string.Format("hitFieldPos : {0} / {1} / {2}", hitFieldPos.x, hitFieldPos.y, hitFieldPos.z));
					_touchedCharacter.SetForceMoveDest(hitFieldPos);
				}

				_touchedCharacter = null;
			}
			else
			{
				Vector3 hitFieldPos = new Vector3();

				do
				{
					if (HitFieldPosition(ref hitFieldPos) == false)
						break;

					if (Vector3.Distance(hitFieldPos, _touchFieldPoint) < 5.0f)
						break;

					Vector3 direction = Vector3.Normalize(hitFieldPos - _touchFieldPoint);
					
					// Vector3.Angle은 0 ~ 90도 사이 값만 나오는 점 참고
					float angleTemp = Vector3.Angle(Vector3.up, direction);
					
					// 0 ~ 360 각도 구하기
					float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
					// 현재 진영이 100 도 저동 기울어지 상태에서 시작하여 하드 코딩으로 보정
					angle -= 100.0f;
					CharacterManager.GetInstance().ChangeFormationForSummonerPos(angle);

				} while (false);

				_touchFieldPoint = Vector3.zero;
			}
		}
			break;

		default:
			break;
		}
	}

	public TouchPhase TouchStateFromMouse()
	{
		switch (_mousePhase)
		{
		
		case TouchPhase.Began:
		{
			if (Input.GetMouseButtonUp(0) == true)
			{
				_mousePhase = TouchPhase.Ended;
			}
			else if (Input.GetMouseButtonUp(0) == false)
			{
				_mousePhase = TouchPhase.Moved;
			}
		}
			break;

		case TouchPhase.Moved:
		case TouchPhase.Stationary:
		{
			if (Input.GetMouseButtonUp(0) == true)
			{
				_mousePhase = TouchPhase.Ended;
			}
		}
			break;

		case TouchPhase.Ended:
		{
			if (Input.GetMouseButtonDown(0) == false)
				_mousePhase = TouchPhase.Canceled;
		}
			break;

		case TouchPhase.Canceled:
		{
			if (Input.GetMouseButtonDown(0) == true)
			{
				_mousePhase = TouchPhase.Began;
				_mouseBeganPosition = Input.mousePosition;
			}
		}
			break;

		default:
			break;
		}

		return _mousePhase;
	}

	public BaseCharacter HitCharacerFromMouse()
	{
		if (Input.GetMouseButtonDown(0) == false)
			return null;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Debug.DrawRay (ray.origin, ray.direction * 100.0f, Color.green);
		
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100.0f) == false)
			return null;

		BaseCharacter character = hit.collider.gameObject.GetComponent<BaseCharacter>();
		if (character == null)
			return null;
		
		return character;
	}

	public BaseCharacter HitCharacerFromTouch()
	{
		/*
		 * TouchPhase
		 * - Began : 스크린에 터치 시작
		 * - Moved : 스크린에 터치 후 이동하는 상태
		 * - Stationary : 터치 후 이동하지 않고 계속 터치하고 있는 상태
		 * - Ended : 터치를 종료했을 때
		 * - Canceled : 터치가 취소됐을 때
		 */

		if (Input.touchCount <= 0)
			return null;

		Touch touch = Input.GetTouch(0);
		Touch touch_temp = Input.touches[0];

		if (touch.phase != TouchPhase.Began)
			return null;

		Ray ray = Camera.main.ScreenPointToRay(touch.position);

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100.0f) == false)
			return null;

		BaseCharacter character = hit.collider.gameObject.GetComponent<BaseCharacter>();
		if (character == null)
			return null;

		return character;
	}

	public bool HitFieldPosition(ref Vector3 fieldPos)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit[] hits = Physics.RaycastAll(ray, 1000.0f);
		if (hits.Length <= 0)
			return false;

		foreach (RaycastHit hit in hits)
		{
			if (hit.collider.tag != "Tag_Field")
				continue;

			fieldPos = hit.point;
			break;
		}

		return true;
	}
}
