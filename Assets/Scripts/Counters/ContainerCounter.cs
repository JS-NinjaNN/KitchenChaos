using System;
using UnityEngine;

public class ContainerCounter : BaseCounter {
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            return;
        }

        KitchenObject.SpawnKitchenObject(_kitchenObjectSO, player);
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}