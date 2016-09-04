using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIStage_Countdown : MonoBehaviour
{
    [SerializeField]
    Text _text = null;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void SetStartCountdownText(int countdown)
    {
        if (countdown > 0)
        {
            _text.text = countdown.ToString();
        }
        else
        {
            _text.text = "GO!";
        }
    }

    void SetFinishCountdownText(int countdown)
    {
        if (countdown > 0)
        {
            _text.text = countdown.ToString();
        }
        else
        {
            _text.text = "Finish!";
        }
    }

    public void DisableCountdown()
    {
        gameObject.SetActive(false);
    }

    public void CountdownAction(float startCountdown, bool isStart, Action completeCountdownAction)
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }

        StartCoroutine(CountdownActionCoroutine(startCountdown, isStart, completeCountdownAction));
    }

    IEnumerator CountdownActionCoroutine(float startCountdown, bool isStart, Action completeCountdownAction)
    {
        float countdown = startCountdown;
        int timeCount = ((int)startCountdown) + 1;

        while (true)
        {
            countdown -= Time.fixedDeltaTime;

            int integerReadyTime = ((int)countdown) + 1;

            if (timeCount != integerReadyTime)
            {
                timeCount = integerReadyTime;
                Debug.LogFormat("timeCount : {0}, readyTime : {1}", timeCount, countdown);

                SetStartCountdownText(timeCount);
            }

            if (countdown > 0f)
            {
                yield return new WaitForFixedUpdate();
                continue;
            }

            if (isStart == true)
            {
                SetStartCountdownText(0);
            }
            else
            {
                SetFinishCountdownText(0);
            }

            break;
        }

        if (completeCountdownAction != null)
        {
            completeCountdownAction();
        }

        Invoke("DisableCountdown", 1f);

        yield break;
    }
}
