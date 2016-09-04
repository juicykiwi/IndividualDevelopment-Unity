using UnityEngine;
using System.Collections;

public class EffectController : MonoBehaviour {

	ParticleSystem _particle = null;

	public GameObject target
	{
		set
		{
			if (value == null)
			{
				transform.SetParent(EffectManager.instance._effectObject.transform);
				return;
			}

			transform.SetParent(value.transform);

			transform.localPosition = Vector3.zero;
		}
	}

	// Method

	void Awake () {
		_particle = this.GetComponentInChildren<ParticleSystem>();
	}

	public void Play()
	{
		_particle.time = 0f;
		Invoke("Stop", _particle.duration);
	}

	public void Stop()
	{
		target = null;
		gameObject.SetActive(false);
	}
}
