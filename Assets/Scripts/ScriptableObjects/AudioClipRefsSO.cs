using UnityEngine;

[CreateAssetMenu]
public class AudioClipRefsSO : ScriptableObject {
    [SerializeField] private AudioClip[] _chop;
    [SerializeField] private AudioClip[] _deliveryFail;
    [SerializeField] private AudioClip[] _deliverySuccess;
    [SerializeField] private AudioClip[] _footstep;
    [SerializeField] private AudioClip[] _objectDrop;
    [SerializeField] private AudioClip[] _objectPickup;
    [SerializeField] private AudioClip[] _trash;
    [SerializeField] private AudioClip[] _warning;
    [SerializeField] private AudioClip _stoveSizzle;

    public AudioClip[] Chop => _chop;
    public AudioClip[] DeliveryFail => _deliveryFail;
    public AudioClip[] DeliverySuccess => _deliverySuccess;
    public AudioClip[] Footstep => _footstep;
    public AudioClip[] ObjectDrop => _objectDrop;
    public AudioClip[] ObjectPickup => _objectPickup;
    public AudioClip[] Trash => _trash;
    public AudioClip[] Warning => _warning;
    public AudioClip StoveSizzle => _stoveSizzle;
}