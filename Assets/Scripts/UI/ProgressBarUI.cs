using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
    [SerializeField] private GameObject _hasProgressGameObject;
    [SerializeField] private Image _barImage;

    private IHasProgress _hasProgress;

    private void Start() {
        _hasProgress = _hasProgressGameObject.GetComponent<IHasProgress>();

        if (_hasProgress == null) {
            Debug.LogError("Game Object " + _hasProgressGameObject + "does not implement IHasProgress interface");
        }

        if (_hasProgress != null) {
            _hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        }

        _barImage.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedArgs e) {
        _barImage.fillAmount = e.ProgressNormalized;

        if (e.ProgressNormalized == 0f || Mathf.Approximately(e.ProgressNormalized, 1f)) {
            Hide();
        } else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}