using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class DeliveryResultUI : MonoBehaviour {
    [SerializeField] private GameObject _content;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failedColor;
    [SerializeField] private Sprite _successSprite;
    [SerializeField] private Sprite _failedSprite;

    private const string POPUP = "Popup";

    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _content.SetActive(false);
    }

    private void Start() {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e) {
        _content.SetActive(true);
        _animator.SetTrigger(POPUP);
        _backgroundImage.color = _failedColor;
        _iconImage.sprite = _failedSprite;
        _messageText.text = "ДОСТАВКА\nПРОВАЛ";
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e) {
        _content.SetActive(true);
        _animator.SetTrigger(POPUP);
        _backgroundImage.color = _successColor;
        _iconImage.sprite = _successSprite;
        _messageText.text = "ДОСТАВКА\nУСПЕХ";
    }
}