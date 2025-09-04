using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress {
    [SerializeField] private CuttingRecipeSO[] _cuttingRecipeSOArray;

    [Min(0)] private int _cuttingProgress;

    public static EventHandler OnAnyCut;
    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressChangedArgs> OnProgressChanged;

    public new static void ResetStaticData() {
        OnAnyCut = null;
    }

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (!player.HasKitchenObject() || !HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                return;

            player.GetKitchenObject().SetKitchenObjectParent(this);
            _cuttingProgress = 0;

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(
                this,
                new IHasProgress.OnProgressChangedArgs {
                    ProgressNormalized = (float)_cuttingProgress / cuttingRecipeSO.CuttingProgressMax
                }
            );
        } else {
            if (player.HasKitchenObject()) {
                if (!player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) return;

                if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    GetKitchenObject().DestroySelf();
            } else
                GetKitchenObject().SetKitchenObjectParent(player);
        }
    }

    public override void InteractAlternate(Player player) {
        if (!HasKitchenObject() || !HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) return;

        _cuttingProgress++;
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

        OnCut?.Invoke(this, EventArgs.Empty);
        OnAnyCut?.Invoke(this, EventArgs.Empty);

        OnProgressChanged?.Invoke(
            this,
            new IHasProgress.OnProgressChangedArgs {
                ProgressNormalized = (float)_cuttingProgress / cuttingRecipeSO.CuttingProgressMax
            }
        );


        if (_cuttingProgress < cuttingRecipeSO.CuttingProgressMax) return;

        KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
        GetKitchenObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        return cuttingRecipeSO != null ? cuttingRecipeSO.Output : null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in _cuttingRecipeSOArray) {
            if (cuttingRecipeSO.Input == inputKitchenObjectSO) return cuttingRecipeSO;
        }

        return null;
    }
}