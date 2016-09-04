using UnityEngine;
using System.Collections;

public class StarCoin : MonoBehaviour
{
    public static StarCoin CreateStarCoin()
    {
        GameObject prefab = Resources.Load("Prefab/StarCoin/StarCoin") as GameObject;
        if (prefab == null)
            return null;

        GameObject newGameObject = Instantiate(prefab) as GameObject;
        StarCoin starCoin = newGameObject.GetComponent<StarCoin>();
        if (starCoin == null)
        {
            Destroy(newGameObject);
            return null;
        }

        return starCoin;
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("Star Coing call OnTriggerEnter2D");
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("Star Coing call OnCollisionEnter2D");
    }
}
