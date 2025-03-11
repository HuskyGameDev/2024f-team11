using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    public static event Action<int> nextNight;
    public static event Action<int> StartNight;

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
        UpdateGameState(GameState.NIGHT_ONE);
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.NIGHT_ONE:
                // Spawn BS
                StartNight?.Invoke(1);
                break;
            case GameState.NIGHT_TWO:
                // Spawn monster
                //Spawn BS
                break;
            case GameState.NIGHT_THREE:
                // Spawn upstairs keys
                // Spawn monster
                // Spawn BS
                break;
            case GameState.NIGHT_FOUR:
                // Spawn basement keys
                // Spawn monster
                // Spawn BS
                break;
            case GameState.NIGHT_FIVE:
                // Spawn front door keys
                // Spawn monster
                // Spawn BS
                break;
            case GameState.GAME_OVER:
                break;
            default:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }
}

public enum GameState
{
    NIGHT_ONE,
    NIGHT_TWO,
    NIGHT_THREE,
    NIGHT_FOUR,
    NIGHT_FIVE,
    GAME_OVER,
}
