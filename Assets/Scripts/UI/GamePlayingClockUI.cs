using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour {
    [SerializeField] private GameObject _content;
    [SerializeField] private Image _timerImage;

    private void Update() {
        _timerImage.fillAmount = GameManager.Instance.GamePlayingTimerNormalized;
    }
}