using UnityEngine;
using System.Collections;

public class FindInstance<T> : MonoBehaviour where T : MonoBehaviour{

	static Object _lock = new Object();
	
	static T _instance = null;

	public static T instance
	{
		get
		{
			lock(_lock)
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<T>();
					if (_instance)
					{
						DontDestroyOnLoad(_instance.gameObject);
					}
				}
				
				return _instance;
			}
		}
	}
}
