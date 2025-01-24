using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}
    private enum State {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private State state;

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 120f;
    private bool isGamePaused = false;



    private void Awake() {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Start() {
        GameInput.Instance.OnPauseAction += GameInputOnPauseAction;
        GameInput.Instance.OnInteractAction += GameInputOnInteractAction;
    }

    private void GameInputOnInteractAction(object sender, EventArgs e)
    {
        if(state == State.WaitingToStart) {
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInputOnPauseAction(object sender, EventArgs e)
    {
        ToggleGamePause();
    }

    private void Update() {
        switch (state)
        {
            case State.WaitingToStart:
                
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if(countdownToStartTimer <= 0) {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);

                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if(gamePlayingTimer <= 0) {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);

                } 
                break;
            case State.GameOver:
                break;
            default:
            break;
        }
    }

    public void ToggleGamePause() {
        if(!IsGameOver()) {
            isGamePaused = !isGamePaused;
            if(isGamePaused) {
                Time.timeScale = 0;
                OnGamePaused?.Invoke(this, EventArgs.Empty);
            } else {
                Time.timeScale = 1;
                OnGameUnpaused?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    public bool IsCountdownActive() {
        return state == State.CountdownToStart;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }

    public float GetCountdownToStartTimer() {
        return countdownToStartTimer;
    }

    public float GetGamePlayingTimer() {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }
}
