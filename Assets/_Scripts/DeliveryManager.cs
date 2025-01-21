using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance {get; private set;}


    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer = 4f;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;

    private void Awake() {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if(waitingRecipeSOList.Count < waitingRecipeMax ) {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOs[Random.Range(0, recipeListSO.recipeSOs.Count)];
                Debug.Log(waitingRecipeSO.name);
                waitingRecipeSOList.Add(waitingRecipeSO);
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
                    Debug.Log("success");
                    waitingRecipeSOList.RemoveAt(i);
                    return;
                }
            }
        }

        Debug.Log("fail");

    }
}
