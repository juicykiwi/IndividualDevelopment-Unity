using UnityEngine;
using System.Collections;

public class GameHelper
{
    public static GameObject Instantiate(GameObject template, GameObject parent = null)
    {
        if (template == null)
            return null;

        GameObject newGameObject = GameObject.Instantiate(template) as GameObject;
        if (parent != null)
        {
            newGameObject.transform.SetParent(parent.transform);
        }

        newGameObject.transform.localPosition = Vector3.zero;
        newGameObject.transform.rotation = Quaternion.identity;
        newGameObject.transform.localScale = Vector3.one;

        return newGameObject;
    }

    public static T Instantiate<T>(T template, GameObject parent = null) where T : MonoBehaviour
    {
        if (template == null)
            return null;

        T newGameObject = GameObject.Instantiate<T>(template);
        if (parent != null)
        {
            newGameObject.transform.SetParent(parent.transform);
        }

        newGameObject.transform.localPosition = Vector3.zero;
        newGameObject.transform.rotation = Quaternion.identity;
        newGameObject.transform.localScale = Vector3.one;

        return newGameObject;
    }
}
