using UnityEngine;

public class MusicManager : MonoBehaviour {
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    private float _volume = 0.5f;
    private AudioSource _audioSource;

    public float Volume => _volume;

    public static MusicManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _audioSource = GetComponent<AudioSource>();
        _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 0.5f);
        _audioSource.volume = _volume;
    }

    public void ChangeVolume() {
        _volume += 0.1f;
        if (_volume > 1.0f) _volume = 0;
        _audioSource.volume = _volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, _volume);
        PlayerPrefs.Save();
    }
}