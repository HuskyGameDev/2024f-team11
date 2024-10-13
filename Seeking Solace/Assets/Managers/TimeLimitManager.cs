using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimitManager : MonoBehaviour
{
    [SerializeField] float MaxTimeLimit;    //In Minutes
    float currentTime;
    float timeSinceChecked;

    private void Update()
    {
        timeSinceChecked += Time.deltaTime;
        calculateTimePassed();

        if (Input.GetKeyDown("q")) calculateDisplay();
    }

    private void calculateTimePassed()
    {
        float maxTimeInSec = MaxTimeLimit * 60;

        //Max time passed is 5 hrs
        float maxTimeInGame = 5 * 60 * 60;

        float ratio = maxTimeInGame / maxTimeInSec;

        currentTime += timeSinceChecked * ratio;
        timeSinceChecked = 0;
    }

    private void calculateDisplay()
    {
        //Start at 6:00
        int intCurrentTime = Mathf.RoundToInt(currentTime);

        int sec = intCurrentTime % 60;
        intCurrentTime -= sec;

        int mins = intCurrentTime % (60 * 60) / 60;
        intCurrentTime -= 60 * mins;

        int hrs = intCurrentTime / (60 * 60) + 6;

        Debug.Log(hrs.ToString() + ":" + mins.ToString("D2") + ":" + sec.ToString("D2"));
    }
}
