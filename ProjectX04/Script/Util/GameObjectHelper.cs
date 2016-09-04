using UnityEngine;
using System.Collections;

public class GameObjectHelper {

	public static T Create<T>() where T : MonoBehaviour
	{
		GameObject obj = new GameObject(typeof(T).Name);
		obj.transform.localPosition = Vector3.zero;
		return obj.AddComponent<T>();
	}
}
