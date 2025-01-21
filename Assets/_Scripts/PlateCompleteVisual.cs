using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [SerializeField] PlateKitchenObject plateKitchenObject;
    [Serializable]
    public struct kitchenObjectSO_GameObject {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject; 
    }

    [SerializeField] private List<kitchenObjectSO_GameObject> kitchenObjectSO_GameObjects;

    private void Start() {
        plateKitchenObject.OnIngredientAdded += plateKitchenObjectOnIngredientAdded;

        foreach (kitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSO_GameObjects) {
            kitchenObjectSO_GameObject.gameObject.SetActive(false); 
        }
    }

    private void plateKitchenObjectOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (kitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSO_GameObjects) {
            if(kitchenObjectSO_GameObject.kitchenObjectSO == e.kitchenObjectSO) {
                kitchenObjectSO_GameObject.gameObject.SetActive(true);
            }
        }
        
    }
}
