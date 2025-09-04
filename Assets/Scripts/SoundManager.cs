using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour {
    [SerializeField] private AudioClipRefsSO _audioClipRefsSO;

    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    private float _volume = 1.0f;

    public float Volume => _volume;

    public static SoundManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) Destroy(gameObject);

        Instance = this;
        _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1.0f);
    }

    private void OnEnable() {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }


    private void OnDisable() {
        DeliveryManager.Instance.OnRecipeSuccess -= DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed -= DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut -= OnAnyCut;
        Player.Instance.OnPickedSomething -= Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere -= BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed -= TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(TrashCounter obj) {
        if (obj != null) PlaySound(_audioClipRefsSO.Trash, obj.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(BaseCounter obj) {
        if (obj != null) PlaySound(_audioClipRefsSO.ObjectDrop, obj.transform.position);
    }

    private void Player_OnPickedSomething(object sender, EventArgs e) {
        PlaySound(_audioClipRefsSO.ObjectPickup, Player.Instance.transform.position);
    }

    private void OnAnyCut(object sender, EventArgs e) {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        if (cuttingCounter != null) PlaySound(_audioClipRefsSO.Chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        if (Camera.main != null) PlaySound(_audioClipRefsSO.DeliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        if (Camera.main != null) PlaySound(_audioClipRefsSO.DeliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1.0f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * _volume);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1.0f) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }


    public void PlayFootstepsSound(Vector3 position, float volume = 1.0f) {
        PlaySound(_audioClipRefsSO.Footstep, position, volume);
    }

    public void PlayCountdownSound() {
        PlaySound(_audioClipRefsSO.Warning, Vector3.zero);
    }

    public void PlayWarningSound(Vector3 position) {
        PlaySound(_audioClipRefsSO.Warning, position);
    }

    public void ChangeVolume() {
        _volume += 0.1f;

        if (_volume > 1.0f) _volume = 0;

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, _volume);
        PlayerPrefs.Save();
    }
}