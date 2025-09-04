using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour {
    [SerializeField] private StoveCounter _stoveCounter;

    private const string IS_FLASHING = "IsFlashing";

    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void Start() {
        _animator.SetBool(IS_FLASHING, false);
        _stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedArgs e) {
        float burnShowProgressAmount = 0.5f;
        bool show = _stoveCounter.IsFried && e.ProgressNormalized >= burnShowProgressAmount;
        _animator.SetBool(IS_FLASHING, show);
    }
}