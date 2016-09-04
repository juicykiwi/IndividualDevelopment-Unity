using UnityEngine;
using System.Collections;

public class RunningMachineController : MonoBehaviour 
{
    [SerializeField]
    SpriteRenderer _upFastButtonSprite = null;

    [SerializeField]
    SpriteRenderer _downFastButtonSprite = null;

    [SerializeField]
    SpriteRenderer _upSlowButtonSprite = null;

    [SerializeField]
    SpriteRenderer _downSlowButtonSprite = null;


    public void SlowButtonDown(int count, float repeatingTime)
    {
        StartCoroutine(SlowButtonDownCoroutine(count, repeatingTime));
    }

    IEnumerator SlowButtonDownCoroutine(int count, float repeatingTime)
    {
        while (count > 0)
        {
            --count;

            _upSlowButtonSprite.enabled = false;
//            _downSlowButtonSprite.enabled = true;

            yield return new WaitForSeconds(repeatingTime);

            _upSlowButtonSprite.enabled = true;
//            _downSlowButtonSprite.enabled = false;

            yield return new WaitForSeconds(repeatingTime);
        }

        yield break;
    }

    public void FastButtonDown(int count, float repeatingTime)
    {
        StartCoroutine(FastButtonDownCoroutine(count, repeatingTime));
    }

    IEnumerator FastButtonDownCoroutine(int count, float repeatingTime)
    {
        while (count > 0)
        {
            --count;

            _upFastButtonSprite.enabled = false;
//            _downFastButtonSprite.enabled = true;

            yield return new WaitForSeconds(repeatingTime);

            _upFastButtonSprite.enabled = true;
//            _downFastButtonSprite.enabled = false;

            yield return new WaitForSeconds(repeatingTime);
        }

        yield break;
    }
}
