using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillLevelData : MonoBehaviour {

	protected int _maxSkillLevel = 0;
	public List<float> _skillLevelValueList = new List<float>();

	void Awake () {
		_maxSkillLevel = _skillLevelValueList.Count;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float IncreaseValueWithSkillLevel(int skillLevel)
	{
		if (skillLevel <= 0)
			return 0.0f;

		if (skillLevel > _maxSkillLevel)
			skillLevel = _maxSkillLevel;

		return _skillLevelValueList[skillLevel - 1];
	}
}
