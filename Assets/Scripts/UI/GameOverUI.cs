using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    [SerializeField] private GameObject _content;
    [SerializeField] private TextMeshProUGUI _recipesDeliveredText;
    [SerializeField] private Button _restartButton;

    private void Awake() {
        _restartButton.onClick.AddListener(GameManager.Instance.RestartGame);
    }

    private void OnEnable() {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
    }

    private void OnDisable() {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
        GameManager.Instance.OnGameRestart -= GameManager_OnGameRestart;
    }

    private void Start() {
        _content.gameObject.SetActive(false);
    }

    private void GameManager_OnGameRestart() {
        _content.gameObject.SetActive(false);
    }

    private void GameManager_OnStateChanged() {
        _content.gameObject.SetActive(GameManager.Instance.IsGameOver);

        if (!GameManager.Instance.IsGameOver) return;

        if (DeliveryManager.Instance != null)
            _recipesDeliveredText.text = DeliveryManager.Instance.SuccessfulRecipesAmount.ToString();
    }
}