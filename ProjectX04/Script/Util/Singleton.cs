using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	static Object _lock = new Object();

	static T _instance = null;

	static bool _isQuit = false;

	public static T instance
	{
		get
		{
			lock(_lock)
			{
				if (_isQuit == true)
				{
					return null;
				}

				if (_instance == null)
				{
					_instance = FindObjectOfType<T>();

					if (_instance == null)
					{
						GameObject singletonObj = new GameObject(typeof(T).Name);
						_instance = singletonObj.AddComponent<T>();
					}

					DontDestroyOnLoad(_instance.gameObject);
				}

				return _instance;
			}
		}
	}

	public void OnApplicationQuit()
	{
		_isQuit = true;
	}

    static public void CreateInstance(Transform parent = null)
	{
		T t = Singleton<T>.instance;

        if (parent != null)
        {
            t.transform.SetParent(parent);
        }
	}
}

public class SingletonNew<T> where T : class, new()
{
	static T _instance = null;
	static public T instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new T();
			}

			return _instance;
		}
	}
}
