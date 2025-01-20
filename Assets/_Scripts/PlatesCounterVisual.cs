using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisual;
    private float offsetAmount = 0.1f;

    private List<GameObject> plateVisualGameObjectList;

    private void Awake() {
        plateVisualGameObjectList = new List<GameObject>();
    }
    private void Start() {
        platesCounter.OnPlateSpawned += platesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += platesCounter_OnPlateRemoved;
    }

    private void platesCounter_OnPlateRemoved(object sender, EventArgs e)
    {
        GameObject lastPlate = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        plateVisualGameObjectList.Remove(lastPlate);
        Destroy(lastPlate);
    }

    private void platesCounter_OnPlateSpawned(object sender, EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisual, counterTopPoint);
        plateVisualTransform.localPosition = new Vector3(0, offsetAmount * plateVisualGameObjectList.Count ,0);
    
        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
