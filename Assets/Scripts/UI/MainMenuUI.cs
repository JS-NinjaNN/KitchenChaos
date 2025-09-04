using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;

    private void Awake() {
        _playButton.onClick.AddListener(() => { Loader.LoadScene(Loader.Scene.GameScene); });
        _quitButton.onClick.AddListener(Application.Quit);

        Time.timeScale = 1.0f;
    }
}