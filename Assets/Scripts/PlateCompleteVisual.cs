using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour {
    [SerializeField] private PlateKitchenObject _plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> _kitchenObjectSOGameObjectList;

    // ReSharper disable once InconsistentNaming
    [Serializable]
    public struct KitchenObjectSO_GameObject {
        [SerializeField] private KitchenObjectSO _kitchenObjectSO;
        [SerializeField] private GameObject _gameObject;

        public KitchenObjectSO KitchenObjectSO => _kitchenObjectSO;
        public GameObject GameObject => _gameObject;
    }

    private void Start() {
        _plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in _kitchenObjectSOGameObjectList) {
            kitchenObjectSOGameObject.GameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in _kitchenObjectSOGameObjectList) {
            if (kitchenObjectSOGameObject.KitchenObjectSO == e.KitchenObjectSO) {
                kitchenObjectSOGameObject.GameObject.SetActive(true);
            }
        }
    }
}