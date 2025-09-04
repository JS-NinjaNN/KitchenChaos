using UnityEngine;

[CreateAssetMenu]
public class CuttingRecipeSO : ScriptableObject {
    [SerializeField] private KitchenObjectSO _input;
    [SerializeField] private KitchenObjectSO _output;
    [SerializeField] private int _cuttingProgressMax;

    public KitchenObjectSO Input => _input;
    public KitchenObjectSO Output => _output;
    public int CuttingProgressMax => _cuttingProgressMax;
}