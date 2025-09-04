using System;
using UnityEngine;

public class PlatesCounter : BaseCounter {
    [SerializeField] private KitchenObjectSO _plateKitchenObjectSO;
    [SerializeField] private float _spawnPlateTimerMax;
    [SerializeField] private int _platesSpawnedAmountMax;

    private float _spawnPlateTimer;
    private int _platesSpawnedAmount;

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    private void Update() {
        {
            _spawnPlateTimer += Time.deltaTime;

            if (!(_spawnPlateTimer > _spawnPlateTimerMax)) return;

            _spawnPlateTimer = 0f;

            if (!GameManager.Instance.IsGamePlaying() || _platesSpawnedAmount >= _platesSpawnedAmountMax) return;

            _platesSpawnedAmount++;

            OnPlateSpawned?.Invoke(this, EventArgs.Empty);
        }
    }

    public override void Interact(Player player) {
        if (player.HasKitchenObject() || _platesSpawnedAmount <= 0) return;

        _platesSpawnedAmount--;
        KitchenObject.SpawnKitchenObject(_plateKitchenObjectSO, player);

        OnPlateRemoved?.Invoke(this, EventArgs.Empty);
    }
}