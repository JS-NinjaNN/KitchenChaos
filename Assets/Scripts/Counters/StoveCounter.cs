using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress {
    [SerializeField] private FryingRecipeSO[] _fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] _burningRecipeSOArray;

    private State _state;
    private float _fryingTimer;
    private float _burningTimer;
    private FryingRecipeSO _fryingRecipeSO;
    private BurningRecipeSO _burningRecipeSO;

    public enum State {
        Idle,
        Frying,
        Fried,
        Burned
    }

    public bool IsFried => _state == State.Fried;
    public event EventHandler<IHasProgress.OnProgressChangedArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedArgs> OnStateChanged;

    public class OnStateChangedArgs : EventArgs {
        public State State;
    }

    private void Start() {
        _state = State.Idle;
        _fryingTimer = 0f;
    }

    private void Update() {
        if (HasKitchenObject()) {
            switch (_state) {
                case State.Idle:
                    break;
                case State.Frying:
                    _fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(
                        this,
                        new IHasProgress.OnProgressChangedArgs {
                            ProgressNormalized = _fryingTimer / _fryingRecipeSO.FryingTimerMax
                        }
                    );

                    if (!(_fryingTimer > _fryingRecipeSO.FryingTimerMax)) return;

                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(_fryingRecipeSO.Output, this);
                    _state = State.Fried;
                    _burningTimer = 0f;
                    _burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    OnStateChanged?.Invoke(this, new OnStateChangedArgs { State = _state });
                    break;
                case State.Fried:
                    _burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(
                        this,
                        new IHasProgress.OnProgressChangedArgs {
                            ProgressNormalized = _burningTimer / _burningRecipeSO.BurningTimerMax
                        }
                    );

                    if (!(_burningTimer > _burningRecipeSO.BurningTimerMax)) return;

                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(_burningRecipeSO.Output, this);
                    _state = State.Burned;
                    OnStateChanged?.Invoke(this, new OnStateChangedArgs { State = _state });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs { ProgressNormalized = 0f });

                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (!player.HasKitchenObject()) return;

            if (!HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) return;

            player.GetKitchenObject().SetKitchenObjectParent(this);
            _fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            _state = State.Frying;
            _fryingTimer = 0f;
            OnStateChanged?.Invoke(this, new OnStateChangedArgs { State = _state });

            OnProgressChanged?.Invoke(
                this,
                new IHasProgress.OnProgressChangedArgs {
                    ProgressNormalized = _fryingTimer / _fryingRecipeSO.FryingTimerMax
                }
            );
        } else {
            if (player.HasKitchenObject()) {
                if (!player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) return;

                if (!plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) return;

                GetKitchenObject().DestroySelf();
                _state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedArgs { State = _state });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs { ProgressNormalized = 0f });
            } else {
                GetKitchenObject().SetKitchenObjectParent(player);
                _state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedArgs { State = _state });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs { ProgressNormalized = 0f });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        if (fryingRecipeSO != null) return fryingRecipeSO.Output;

        return null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in _fryingRecipeSOArray) {
            if (fryingRecipeSO.Input == inputKitchenObjectSO) return fryingRecipeSO;
        }

        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in _burningRecipeSOArray) {
            if (burningRecipeSO.Input == inputKitchenObjectSO) return burningRecipeSO;
        }

        return null;
    }
}