using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _countdownText;

    private const string NUMBER_POPUP = "NumberPopup";

    private Animator _animator;
    private int _previousCountdownNumber;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        if (GameManager.Instance != null) GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDisable() {
        if (GameManager.Instance != null) GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void Start() {
        _countdownText.gameObject.SetActive(false);
    }

    private void Update() {
        if (GameManager.Instance == null) return;

        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.RemainingSecondsToStart);
        _countdownText.text = countdownNumber.ToString(CultureInfo.InvariantCulture);

        if (_previousCountdownNumber == countdownNumber) return;
        _previousCountdownNumber = countdownNumber;
        _animator.SetTrigger(NUMBER_POPUP);
        SoundManager.Instance.PlayCountdownSound();
    }

    private void GameManager_OnStateChanged() {
        _countdownText.gameObject.SetActive(GameManager.Instance.IsCountdownToStartActive);
    }
}