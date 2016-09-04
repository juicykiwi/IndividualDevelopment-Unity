using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance = null;

	static Object _lock = new Object();

    // Method

	public static T instance
	{
		get
		{
			if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();

                        if (_instance == null)
                        {
                            GameObject singletonObj = new GameObject(typeof(T).Name);
                            _instance = singletonObj.AddComponent<T>();
                        }

                        if (_instance == null)
                        {
                            Debug.Log("_instance is null : " + typeof(T).Name);
                        }

                        _instance.transform.position = Vector3.zero;
                        _instance.transform.rotation = Quaternion.identity;

                        DontDestroyOnLoad(_instance.gameObject);
                    }
                }
			}

			return _instance;
		}
	}

	static public void CreateInstance()
	{
		T t = Singleton<T>.instance;
		Debug.Log("CreateInstance : " + t.name);
	}
}

public class SingletonNew<T> where T : class, new()
{
	static T _instance = null;

    static object _lock = new object();

    // Method

	static public T instance
	{
		get
		{
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }

			return _instance;
		}
	}
}

public class SingletonFindBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	static T _instance = null;

    static Object _lock = new Object();

    // Method
	
	public static T instance
	{
		get
		{
			if (_instance == null)
			{
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();

                        if (_instance != null)
                        {
                            _instance.transform.position = Vector3.zero;
                            _instance.transform.rotation = Quaternion.identity;
                        }
                    }
                }
			}
			
			return _instance;
		}
	}
}
