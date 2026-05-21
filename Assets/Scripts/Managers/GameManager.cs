using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool showCompletionPopup = false;

    public enum GameState
    {
        Playing,
        Paused,
        LevelFinished,
        TimeUp,
        FellOff,
        MainMenu
    }

    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetState(GameState.Playing);
    }

    private void Update()
    {
        if (CurrentState != GameState.Playing &&
            CurrentState != GameState.Paused)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == GameState.Playing)
                SetState(GameState.Paused);
            else
                SetState(GameState.Playing);
        }
    }

    private void SetState(GameState newState)
    {
        CurrentState = newState;

        Time.timeScale = (newState == GameState.Paused) ? 0f : 1f;

        OnGameStateChanged?.Invoke(newState);
    }

    private bool ShouldTimeBeFrozen(GameState state)
    {
        return state != GameState.Playing;
    }

    public void FinishLevel() => SetState(GameState.LevelFinished);
    public void TimeUp() => SetState(GameState.TimeUp);
    public void FallDeath() => SetState(GameState.FellOff);

    public void Resume() => SetState(GameState.Playing);
    public void Pause() => SetState(GameState.Paused);

    public void MainMenu() => SetState(GameState.MainMenu);
    
}
