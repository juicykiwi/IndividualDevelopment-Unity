using UnityEngine;
using System.Collections;

public class UILogoPanel : MonoBehaviour {

	Animator _animator = null;

	// Method

	void Awake () {
		_animator = this.GetComponent<Animator>();
		if (_animator)
		{
			_animator.speed = 0f;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartLogo()
	{
		if (_animator)
		{
			_animator.speed = 1f;
		}
	}

	public void OnEndLogo()
	{
		SceneLogo.instance.DoNextScene();
	}
}
