using UnityEngine;

[CreateAssetMenu]
public class BurningRecipeSO : ScriptableObject {
    [SerializeField] private KitchenObjectSO _input;
    [SerializeField] private KitchenObjectSO _output;
    [SerializeField] private float _burningTimerMax;

    public KitchenObjectSO Input => _input;
    public KitchenObjectSO Output => _output;
    public float BurningTimerMax => _burningTimerMax;
}