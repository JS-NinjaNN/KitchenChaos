using UnityEngine;

public class PlayerSounds : MonoBehaviour {
    private Player _player;
    private float _footstepTimer;
    private float _footstepTimerMax = 0.1f;

    private void Awake() {
        _player = GetComponent<Player>();
    }

    private void Update() {
        _footstepTimer -= Time.deltaTime;

        if (!(_footstepTimer <= 0)) return;
        _footstepTimer = _footstepTimerMax;

        if (!_player.IsWalking()) return;
        const float volume = 1.0f;
        SoundManager.Instance.PlayFootstepsSound(_player.transform.position, volume);
    }
}