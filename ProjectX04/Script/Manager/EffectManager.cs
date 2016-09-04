using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectManager : ManagerBase<EffectManager> {

	string EffectObjectName = "EffectList";
	public GameObject _effectObject = null;

	Dictionary<string, List<EffectController>> _effectListDict = new Dictionary<string, List<EffectController>>();

	// Method

	protected override void Awake () {
		base.Awake();
		
		_effectObject = new GameObject(EffectObjectName);
		_effectObject.transform.SetParent(this.transform);
	}

	public override void ActionSceneClosed(SceneType sceneType)
	{
		SceneType[] typeArray = new SceneType[] { SceneType.GameStage };
		
		if (IsHaveSceneTypeFormArray(sceneType, typeArray) == true)
		{
			_effectListDict.Clear();
		}
	}

	public void ClearEffect()
	{
		_effectListDict.Clear();
	}

	void PushEffect(string effectName)
	{
		PushEffect(effectName, 1);
	}

	void PushEffect(string effectName, int count)
	{
		GameObject effectPrefab = LoadDataManager.instance._effectPrefabDict.GetPrefabByName(effectName);
		if (effectPrefab == null)
			return;

		EffectController effectController = effectPrefab.GetComponent<EffectController>();
		if (effectController == null)
			return;

		if (_effectListDict.ContainsKey(effectName) == false)
		{
			List<EffectController> newEffectList = new List<EffectController>();
			_effectListDict[effectName] = newEffectList;
		}

		for (int i = 0; i < count; ++i)
		{
			List<EffectController> effectList = _effectListDict[effectName];

			EffectController newEffectController = Instantiate<EffectController>(effectController);
			newEffectController.gameObject.SetActive(false);
			newEffectController.transform.SetParent(_effectObject.transform);

			effectList.Add(newEffectController);
		}
	}

	public EffectController GetEffect(string effectName)
	{
		if (_effectListDict.ContainsKey(effectName) == false)
		{
			PushEffect(effectName);

			if (_effectListDict.ContainsKey(effectName) == false)
			{
				return null;
			}
		}

		List<EffectController> effectList = _effectListDict[effectName];

		EffectController effectController = effectList.Find(
			(EffectController effect) => { return effect.gameObject.activeSelf == false; });

		if (effectController == null)
		{
			PushEffect(effectName);

			effectController = effectList.Find(
				(EffectController effect) => { return effect.gameObject.activeSelf == false; });
		}

		return effectController;
	}

	public void PlayEffect(string effectName, Vector2 pos)
	{
		EffectController effectController = GetEffect(effectName);
		if (effectController == null)
			return;

		effectController.gameObject.SetActive(true);
		effectController.transform.position = new Vector3(pos.x, pos.y);
		effectController.Play();
	}

	public void PlayEffect(string effectName, GameObject target, Vector2 LocalPos)
	{
		EffectController effectController = GetEffect(effectName);
		if (effectController == null)
			return;
		
		effectController.gameObject.SetActive(true);
		effectController.target = target;
		effectController.transform.localPosition = new Vector3(LocalPos.x, LocalPos.y);
		effectController.Play();
	}
}
