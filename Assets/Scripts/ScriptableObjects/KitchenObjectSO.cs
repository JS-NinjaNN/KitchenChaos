using UnityEngine;

[CreateAssetMenu]
public class KitchenObjectSO : ScriptableObject {
    [SerializeField] private Transform _prefab;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _objectName;

    public Transform Prefab => _prefab;
    public Sprite Sprite => _sprite;
    public string ObjectName => _objectName;
}