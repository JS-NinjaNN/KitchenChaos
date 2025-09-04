using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour {
    [SerializeField] private RecipeListSO _recipeListSO;
    [SerializeField] private float _spawnRecipeTimerMax;
    [SerializeField] private int _waitingRecipesMax;

    private static DeliveryManager _instance;
    private List<RecipeSO> _waitingRecipeSOList;
    private float _spawnRecipeTimer;
    private int _successfulRecipesAmount;

    public static DeliveryManager Instance => _instance;
    public int SuccessfulRecipesAmount => _successfulRecipesAmount;
    public List<RecipeSO> WaitingRecipeSOList => _waitingRecipeSOList;
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeFailed;
    public event EventHandler OnRecipeSuccess;


    private void Awake() {
        if (_instance == null) _instance = this;

        _waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update() {
        _spawnRecipeTimer -= Time.deltaTime;

        if (!(_spawnRecipeTimer <= 0f)) return;
        _spawnRecipeTimer = _spawnRecipeTimerMax;

        if (!GameManager.Instance.IsGamePlaying() || _waitingRecipeSOList.Count >= _waitingRecipesMax) return;
        RecipeSO waitingRecipeSO = _recipeListSO.RecipeSOList[Random.Range(0, _recipeListSO.RecipeSOList.Count)];
        _waitingRecipeSOList.Add(waitingRecipeSO);

        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void OnEnable() {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
    }

    private void OnDisable() {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnGameRestart -= GameManager_OnGameRestart;
    }

    private void GameManager_OnGameRestart() {
        _waitingRecipeSOList = new List<RecipeSO>();
        _successfulRecipesAmount = 0;
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for (int i = 0; i < _waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSO = _waitingRecipeSOList[i];

            if (waitingRecipeSO.KitchenObjectSOList.Count != plateKitchenObject.KitchenObjectSOList.Count) continue;
            bool plateContentsMatchesRecipe = true;

            foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.KitchenObjectSOList) {
                bool ingredientFound = false;

                foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.KitchenObjectSOList) {
                    if (plateKitchenObjectSO != recipeKitchenObjectSO) continue;
                    ingredientFound = true;
                    break;
                }

                if (!ingredientFound) plateContentsMatchesRecipe = false;
            }

            if (!plateContentsMatchesRecipe) continue;
            _waitingRecipeSOList.RemoveAt(i);
            OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
            OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
            _successfulRecipesAmount++;
            return;
        }

        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }
}