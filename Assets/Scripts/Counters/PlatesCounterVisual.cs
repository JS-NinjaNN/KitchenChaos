using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour {
    [SerializeField] private Transform _counterTopPoint;
    [SerializeField] private Transform _plateVisualPrefab;
    [SerializeField] private PlatesCounter _platesCounter;

    private List<GameObject> _plateVisualGameObjectList;

    private void Awake() {
        _plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start() {
        _platesCounter.OnPlateSpawned += PlatesCounter_OnPlatesSpawned;
        _platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e) {
        GameObject plateGameObject = _plateVisualGameObjectList[^1];
        _plateVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void PlatesCounter_OnPlatesSpawned(object sender, EventArgs e) {
        Transform plateVisualTransform = Instantiate(_plateVisualPrefab, _counterTopPoint);

        float plateOffsetY = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * _plateVisualGameObjectList.Count, 0);
        _plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}