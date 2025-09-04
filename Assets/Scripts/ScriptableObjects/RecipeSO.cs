using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecipeSO : ScriptableObject {
    [SerializeField] private List<KitchenObjectSO> _kitchenObjectSOList;
    [SerializeField] private string _recipeName;

    public IReadOnlyList<KitchenObjectSO> KitchenObjectSOList => _kitchenObjectSOList;
    public string RecipeName => _recipeName;
}