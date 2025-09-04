using System;
using UnityEngine;

[DefaultExecutionOrder(-99)] // гарантируем, что Awake сработает пораньше
public class GameManager : MonoBehaviour {
    private enum State {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private State _state;
    private float _countdownToStartTimer = 3.0f;
    private float _gamePlayingTimer;
    private float _gamePlayingTimerMax = 300.0f;
    private bool _isGamePaused;

    public static GameManager Instance { get; private set; }
    public event Action OnStateChanged;
    public event Action<bool> OnPauseChanged;
    public event Action OnGameRestart;
    public bool IsCountdownToStartActive => _state is State.CountdownToStart;
    public bool IsGameOver => _state is State.GameOver;
    public float RemainingSecondsToStart => _countdownToStartTimer;
    public float GamePlayingTimerNormalized => 1 - _gamePlayingTimer / _gamePlayingTimerMax;

    private void OnEnable() {
        if (GameInput.Instance == null) return;
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void OnDisable() {
        if (GameInput.Instance == null) return;
        GameInput.Instance.OnPauseAction -= GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction -= GameInput_OnInteractAction;
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _state = State.WaitingToStart;
    }

    private void Update() {
        switch (_state) {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                _countdownToStartTimer -= Time.deltaTime;
                if (_countdownToStartTimer <= 0f) _state = State.GamePlaying;
                _gamePlayingTimer = _gamePlayingTimerMax;
                OnStateChanged?.Invoke();
                break;
            case State.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;
                if (_gamePlayingTimer <= 0f) _state = State.GameOver;
                OnStateChanged?.Invoke();
                break;
            case State.GameOver:
                break;
            default:
                Debug.LogError("Unhandled state: " + _state);
                break;
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (_state == State.WaitingToStart) {
            _state = State.CountdownToStart;
            OnStateChanged?.Invoke();
        }
    }


    private void GameInput_OnPauseAction() {
        TogglePauseGame();
    }

    public void RestartGame() {
        _state = State.CountdownToStart;
        _countdownToStartTimer = 3.0f;
        _gamePlayingTimer = 0f;
        OnGameRestart?.Invoke();
    }

    public bool IsGamePlaying() {
        return _state == State.GamePlaying;
    }

    public void TogglePauseGame() {
        _isGamePaused = !_isGamePaused;
        Time.timeScale = _isGamePaused ? 0f : 1.0f;

        OnPauseChanged?.Invoke(_isGamePaused);
    }
}