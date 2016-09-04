using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BattleState
{
	Battling,
	Win,
	Lose,
}

public class SceneBattlePlay : MonoBehaviour {

	public static SceneBattlePlay GetInstance() { return _instance; }
	protected static SceneBattlePlay _instance = null;

	public BattleState _battleState = BattleState.Battling;

	public string _nextSceneName = "";

	public float _checkBattleEndTime = 1.0f;
	public float _changeSceneTime = 1.0f;

	protected UIBattleResult _uiBattleResult = null;

	void Awake () {
		if (_instance == null)
		{
			_instance = this;
		}

		_uiBattleResult = FindObjectOfType<UIBattleResult>();
	}

	// Use this for initialization
	void Start () {
		StartCoroutine("CoroutineCheckBattleEnd");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DoNextScene()
	{
		if (_battleState == BattleState.Win)
		{
			SkillManager.GetInstance()._skillEnchantPoint += 5;
		}

		Application.LoadLevel(_nextSceneName);
	}

	IEnumerator CoroutineCheckBattleEnd()
	{
		bool isLoop = true;
		do
		{
			yield return new WaitForSeconds(_checkBattleEndTime);
			
			BattleState battleState = CurrentBattleState();
			
			switch (battleState)
			{
			case BattleState.Win:
			{
				yield return new WaitForSeconds(2.0f);

				BattleState battleStateRetry = CurrentBattleState();
				if (battleStateRetry != battleState)
					break;

				Debug.Log("Win!!!");
				isLoop = false;

				_uiBattleResult._spriteWin.SetActive(true);
				
				if (SceneBattlePlay.GetInstance())
				{
					SceneBattlePlay.GetInstance()._battleState = battleState;
					SceneBattlePlay.GetInstance().Invoke("DoNextScene", _changeSceneTime);
				}
			}
				break;
				
			case BattleState.Lose:
			{
				BattleState battleStateRetry = CurrentBattleState();
				if (battleStateRetry != battleState)
					break;

				Debug.Log("Lose!!!");
				isLoop = false;

				_uiBattleResult._spriteLose.SetActive(true);
				
				if (SceneBattlePlay.GetInstance())
				{
					SceneBattlePlay.GetInstance()._battleState = battleState;
					SceneBattlePlay.GetInstance().Invoke("DoNextScene", _changeSceneTime);
				}
			}
				break;
				
			default:
				break;
			}
			
		} while (isLoop);
		
		yield return null;
	}

	public BattleState CurrentBattleState()
	{
		List<BaseCharacter> allyCharacters = CharacterManager.GetInstance().EqualBattleSideCharacters(BattleSide.A);
		if (allyCharacters.Count <= 0)
		{
			return BattleState.Lose;
		}
		
		List<BaseCharacter> enemyCharacters = CharacterManager.GetInstance().EqualBattleSideCharacters(BattleSide.B);
		if (enemyCharacters.Count <= 0)
		{
			return BattleState.Win;
		}
		
		return BattleState.Battling;
	}
}
