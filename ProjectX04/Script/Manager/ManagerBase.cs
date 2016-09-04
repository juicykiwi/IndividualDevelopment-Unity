using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerBase<T> : Singleton<T> where T : MonoBehaviour {

	// Method

	protected virtual void Awake()
	{
		SceneManager.instance._actionSceneLoaded += ActionSceneLoaded;
		SceneManager.instance._actionSceneClosed += ActionSceneClosed;
	}

    public void SetParentForManagerController()
    {
        transform.SetParent(ManagerController.instance.transform);
    }

	public virtual void ActionSceneLoaded(SceneType sceneType)
	{

	}

	public virtual void ActionSceneClosed(SceneType sceneType)
	{

	}

	public bool IsHaveSceneTypeFormArray(SceneType TargetType, SceneType[] typeArray)
	{
		foreach (SceneType type in typeArray)
		{
			if (TargetType == type)
			{
				return true;
			}
		}

		return false;
	}
}
