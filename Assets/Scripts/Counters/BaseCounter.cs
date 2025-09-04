using System;
using UnityEngine;

[SelectionBase]
public class BaseCounter : MonoBehaviour, IKitchenObjectParent {
    [SerializeField] private Transform _counterTopPoint;

    private KitchenObject _kitchenObject;

    public static event Action<BaseCounter> OnAnyObjectPlacedHere;

    public static void ResetStaticData() {
        OnAnyObjectPlacedHere = null;
    }

    public virtual void Interact(Player player) {
        Debug.LogError("BaseCounter.Interact()");
    }

    public virtual void InteractAlternate(Player player) {
        // Debug.LogError("BaseCounter.InteractAlternate()");
    }

    public Transform GetKitchenObjectFollowTransform() {
        return _counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        _kitchenObject = kitchenObject;
        if (kitchenObject != null) OnAnyObjectPlacedHere?.Invoke(this);
    }

    public KitchenObject GetKitchenObject() {
        return _kitchenObject;
    }

    public void ClearKitchenObject() {
        _kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return _kitchenObject != null;
    }
}