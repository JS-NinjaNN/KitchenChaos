using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {
    [SerializeField] private List<KitchenObjectSO> _validKitchenObjectSOList;

    private List<KitchenObjectSO> _kitchenObjectSOList;

    public IReadOnlyList<KitchenObjectSO> KitchenObjectSOList => _kitchenObjectSOList;

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs {
        public KitchenObjectSO KitchenObjectSO;
    }

    private void Awake() {
        _kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO) {
        if (!_validKitchenObjectSOList.Contains(kitchenObjectSO)) {
            return false;
        }

        if (_kitchenObjectSOList.Contains(kitchenObjectSO)) {
            return false;
        }

        _kitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { KitchenObjectSO = kitchenObjectSO });
        return true;
    }
}