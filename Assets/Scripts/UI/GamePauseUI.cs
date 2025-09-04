using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour {
    [SerializeField] private GameObject _content;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _optionsButton;

    private void Awake() {
        _content.SetActive(false);
        _mainMenuButton.onClick.AddListener(() => Loader.LoadScene(Loader.Scene.MainMenuScene));
        _resumeButton.onClick.AddListener(GameManager.Instance.TogglePauseGame);
        _optionsButton.onClick.AddListener(OptionsUI.Instance.ToggleOptionsWindow);
    }

    private void OnEnable() {
        GameManager.Instance.OnPauseChanged += GameManager_OnPauseChanged;
    }

    private void OnDisable() {
        GameManager.Instance.OnPauseChanged += GameManager_OnPauseChanged;
    }

    private void GameManager_OnPauseChanged(bool isGamePaused) {
        _content.SetActive(isGamePaused);
    }
}