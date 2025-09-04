using UnityEngine;

public class StoveCounterVisual : MonoBehaviour {
    [SerializeField] private StoveCounter _stoveCounter;
    [SerializeField] private GameObject _stoveOnGameObject;
    [SerializeField] private GameObject _particlesGameObject;

    private void Start() {
        _stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedArgs e) {
        bool showVisual = e.State == StoveCounter.State.Frying || e.State == StoveCounter.State.Fried;

        _stoveOnGameObject.SetActive(showVisual);
        _particlesGameObject.SetActive(showVisual);
    }
}