using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance {get; private set;}

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeDelivered;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFail;


    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer = 4f;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int succesfulDeliveries = 0;

    private void Awake() {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if(waitingRecipeSOList.Count < waitingRecipeMax ) {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOs[UnityEngine.Random.Range(0, recipeListSO.recipeSOs.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {
                bool isPlateContentMatches = true;
                foreach (KitchenObjectSO waitingObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO deliveryObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if(waitingObjectSO == deliveryObjectSO) {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if(!ingredientFound) {
                        isPlateContentMatches = false;
                    }
                }
                if(isPlateContentMatches) {
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeDelivered?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    succesfulDeliveries++;
                    return;
                }
            }

        }

        OnRecipeFail?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetRecipeSOList() {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulDeliveries() {
        return succesfulDeliveries;
    }
}
