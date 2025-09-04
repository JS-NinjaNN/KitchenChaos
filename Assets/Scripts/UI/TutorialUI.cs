using System;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour {
    [SerializeField] private GameObject _content;
    [SerializeField] private TextMeshProUGUI _keyMoveUpText;
    [SerializeField] private TextMeshProUGUI _keyMoveDownText;
    [SerializeField] private TextMeshProUGUI _keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI _keyMoveRightText;
    [SerializeField] private TextMeshProUGUI _keyInteractText;
    [SerializeField] private TextMeshProUGUI _keyInteractAlternateText;

    private void OnEnable() {
        if (GameInput.Instance != null) GameInput.Instance.OnRebindStateChanged += GameInput_OnRebindStateChanged;
        if (GameManager.Instance != null) GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDisable() {
        if (GameInput.Instance != null) GameInput.Instance.OnRebindStateChanged -= GameInput_OnRebindStateChanged;
        if (GameManager.Instance != null) GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged() {
        if (GameManager.Instance.IsCountdownToStartActive) _content.SetActive(false);
    }

    private void GameInput_OnRebindStateChanged(GameInput.Binding _, bool __) {
        UpdateVisual();
    }

    private void Start() {
        UpdateVisual();
        _content.SetActive(true);
    }

    private void UpdateVisual() {
        _keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        _keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        _keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        _keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        _keyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        _keyInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
    }
}