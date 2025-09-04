using UnityEngine;

[CreateAssetMenu]
public class FryingRecipeSO : ScriptableObject {
    [SerializeField] private KitchenObjectSO _input;
    [SerializeField] private KitchenObjectSO _output;
    [SerializeField] private float _fryingTimerMax;

    public KitchenObjectSO Input => _input;
    public KitchenObjectSO Output => _output;
    public float FryingTimerMax => _fryingTimerMax;
}