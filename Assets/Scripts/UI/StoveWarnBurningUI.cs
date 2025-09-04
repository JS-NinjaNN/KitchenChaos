using System;
using UnityEngine;

public class StoveWarnBurningUI : MonoBehaviour {
    [SerializeField] private StoveCounter _stoveCounter;

    private void Start() {
        gameObject.SetActive(false);
        _stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedArgs e) {
        float burnShowProgressAmount = 0.5f;
        bool show = _stoveCounter.IsFried && e.ProgressNormalized >= burnShowProgressAmount;
        gameObject.SetActive(show);
    }
}