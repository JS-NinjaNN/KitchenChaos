using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {
    [SerializeField] private GameObject _content;
    [SerializeField] private Button _soundEffectsButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _moveUpButton;
    [SerializeField] private Button _moveDownButton;
    [SerializeField] private Button _moveLeftButton;
    [SerializeField] private Button _moveRightButton;
    [SerializeField] private Button _interactButton;
    [SerializeField] private Button _interactAlternateButton;
    [SerializeField] private TextMeshProUGUI _soundEffectsText;
    [SerializeField] private TextMeshProUGUI _musicText;
    [SerializeField] private TextMeshProUGUI _moveUpText;
    [SerializeField] private TextMeshProUGUI _moveDownText;
    [SerializeField] private TextMeshProUGUI _moveLeftText;
    [SerializeField] private TextMeshProUGUI _moveRightText;
    [SerializeField] private TextMeshProUGUI _interactText;
    [SerializeField] private TextMeshProUGUI _interactAlternateText;
    [SerializeField] private Transform _pressToRebindTransform;

    private bool _isOptionsShown;

    public static OptionsUI Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _soundEffectsButton.onClick.AddListener(() => {
                SoundManager.Instance.ChangeVolume();
                UpdateVisual();
            }
        );

        _musicButton.onClick.AddListener(() => {
                MusicManager.Instance.ChangeVolume();
                UpdateVisual();
            }
        );

        _closeButton.onClick.AddListener(ToggleOptionsWindow);
        _moveUpButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.MoveUp));
        _moveDownButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.MoveDown));
        _moveLeftButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.MoveLeft));
        _moveRightButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.MoveRight));
        _interactButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Interact));
        _interactAlternateButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.InteractAlternate));
    }

    private void Start() {
        UpdateVisual();
        _content.SetActive(false);
        _pressToRebindTransform.gameObject.SetActive(false);
    }

    private void OnEnable() {
        GameManager.Instance.OnPauseChanged += GameManager_OnPauseChanged;
        GameInput.Instance.OnRebindStateChanged += GameInput_OnRebindStateChanged;
    }

    private void OnDisable() {
        GameManager.Instance.OnPauseChanged -= GameManager_OnPauseChanged;
        GameInput.Instance.OnRebindStateChanged -= GameInput_OnRebindStateChanged;
    }


    private void GameInput_OnRebindStateChanged(GameInput.Binding _, bool isWaiting) {
        _pressToRebindTransform.gameObject.SetActive(isWaiting);

        if (!isWaiting) UpdateVisual();
    }


    private void GameManager_OnPauseChanged(bool isGamePaused) {
        if (isGamePaused) return;
        _isOptionsShown = false;
        _content.SetActive(false);
    }

    private void UpdateVisual() {
        _soundEffectsText.text = "ЗВУКОВЫЕ ЭФФЕКТЫ: " + Mathf.Round(SoundManager.Instance.Volume * 10f);
        _musicText.text = "МУЗЫКА: " + Mathf.Round(MusicManager.Instance.Volume * 10f);

        _moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        _moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        _moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        _moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        _interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        _interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
    }

    private void RebindBinding(GameInput.Binding binding) {
        GameInput.Instance.RebindBinding(binding);
    }

    public void ToggleOptionsWindow() {
        _isOptionsShown = !_isOptionsShown;
        _content.SetActive(_isOptionsShown);
    }
}