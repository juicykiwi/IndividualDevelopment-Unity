using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameObjectHelper : MonoBehaviour
{
    // Method

	public static T NewGameObject<T>() where T : MonoBehaviour
	{
		GameObject newObject = new GameObject(typeof(T).Name);
		newObject.transform.localPosition = Vector3.zero;
		newObject.transform.rotation = Quaternion.identity;
		return newObject.AddComponent<T>();
	}

    public static T NewGameObject<T>(T original) where T : MonoBehaviour
    {
        T newObject = GameObject.Instantiate<T>(original);
        if (newObject == null)
            return null;
        
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.rotation = Quaternion.identity;
        return newObject;
    }

	public static GameObject NewEmptyGameObject(string name)
	{
		GameObject newObject = new GameObject(name);
		newObject.transform.localPosition = Vector3.zero;
		newObject.transform.rotation = Quaternion.identity;
		return newObject;
	}

	// Destroy

	public static void DestroyAll<T>() where T : MonoBehaviour
	{
		List<T> tList = new List<T>(FindObjectsOfType<T>());

		foreach (T t in tList)
		{
			if (t == null)
				continue;

			Destroy(t.gameObject);
		}
	}

	public static void DestroyChildAll<T>(Transform trans) where T : MonoBehaviour
	{
        T[] tChildren = trans.GetComponentsInChildren<T>(true);

		foreach (T tChild in tChildren)
		{
			if (tChild == null)
				continue;

			Destroy(tChild.gameObject);
		}
	}

	// Abs position

	public static Vector2 RoundPosition2D(Vector2 pos)
	{
		float posX = Mathf.RoundToInt(pos.x);
		float posY = Mathf.RoundToInt(pos.y);

		return new Vector2(posX, posY);
	}

	public static Vector2 RoundPosition2D(Vector3 pos)
	{
		float posX = Mathf.RoundToInt(pos.x);
		float posY = Mathf.RoundToInt(pos.y);
		
		return new Vector2(posX, posY);
	}

	// Rect

	public static Rect RectByTransform(Transform trans)
	{
		float minPosX = trans.localPosition.x - (trans.localScale.x * 0.5f);
		float minPosY = trans.localPosition.y - (trans.localScale.y * 0.5f);

		return new Rect(new Vector2(minPosX, minPosY), trans.localScale);
	}

	// Angle

	public static float CosByDegree(float degree)
	{
		degree = degree % 360;
		float cosValue = Mathf.Cos(Mathf.Deg2Rad * degree);

		if (Mathf.Abs(cosValue) * 1000f < 1f)
			return 0f;

		return cosValue;
	}

	public static float SinByDegree(float degree)
	{
		degree = degree % 360;
		float sinValue = Mathf.Sin(Mathf.Deg2Rad * degree);

		if (Mathf.Abs(sinValue) * 1000f < 1f)
			return 0f;
		
		return sinValue;
	}
}




