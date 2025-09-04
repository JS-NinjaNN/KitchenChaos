using System;
using UnityEngine;

public class TrashCounter : BaseCounter {
    public static event Action<TrashCounter> OnAnyObjectTrashed;

    public new static void ResetStaticData() {
        OnAnyObjectTrashed = null;
    }

    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) return;
        player.GetKitchenObject().DestroySelf();
        OnAnyObjectTrashed?.Invoke(this);
    }
}