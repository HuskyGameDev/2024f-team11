using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int nightNum;

    public static event Action<int> nextNight;

    #region Singleton Setup
    public static GameManager Instance { get; private set; }

    // If there is an instance, and it's not me, destroy myself.

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        nightNum = 0;
        nextNightCall();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            nextNightCall();
    }
    private void OnLevelWasLoaded(int level)
    {
        if (level == 0)
            nextNightCall();
    }

    public void nextNightCall()
    {
        Debug.Log("Calling next night with night " + nightNum);
        nightNum++;
        nextNight?.Invoke(nightNum);
    }

    public int getNightNum()
    {
        return nightNum;
    }
}
