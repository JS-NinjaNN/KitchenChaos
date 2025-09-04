using System;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour {
    [SerializeField] private StoveCounter _stoveCounter;

    private AudioSource _audioSource;
    private float _warningSoundTimer;
    private bool _playWarningSound;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        _stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        _stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }


    private void Update() {
        if (!_playWarningSound) return;

        _warningSoundTimer -= Time.deltaTime;

        if (!(_warningSoundTimer <= 0)) return;
        float warningSoundTimerMax = 0.2f;
        _warningSoundTimer = warningSoundTimerMax;
        SoundManager.Instance.PlayWarningSound(_stoveCounter.transform.position);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedArgs e) {
        float burnShowProgressAmount = 0.5f;
        _playWarningSound = _stoveCounter.IsFried && e.ProgressNormalized >= burnShowProgressAmount;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedArgs e) {
        bool playSound = e.State is StoveCounter.State.Frying or StoveCounter.State.Fried;

        if (playSound)
            _audioSource.Play();
        else
            _audioSource.Pause();
    }
}