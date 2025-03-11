using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    int currentNightNum = 0;

    public Animator player;

    public static event Action<int> AnimationStarted;
    public static event Action<int> AnimationStopped;
    private void OnEnable()
    {
        GameManager.StartNight += PlayWakeUpAnimation;
    }

    private void OnDisable()
    {
        GameManager.StartNight -= PlayWakeUpAnimation;
    }

    private void PlayWakeUpAnimation(int nightNum)
    {
        currentNightNum = nightNum;
        player.SetTrigger("Wake");
    }

    public void PauseFeedback()
    {
        AnimationStarted?.Invoke(currentNightNum);
    }

    public void ResumeFeedback()
    {
        AnimationStopped?.Invoke(currentNightNum);
    }


}
