using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    private enum State {
        Idle,
        Cooking,
        Cooked,
        Burned
    }

    [SerializeField] private CookingRecipeSO[] cookingRecipes;
    private float cookingProcess = 0f;
    private CookingRecipeSO cookingRecipeSO;
    private State currentState;
    private void Start() {
        currentState = State.Idle;
    }
    private void Update() {
        if(HasKitchenObject()) {
            switch (currentState)
            {
                case State.Idle:
                case State.Cooking:
                    cookingProcess += Time.deltaTime;
                    if(cookingRecipeSO != null) {
                        if(cookingProcess >= cookingRecipeSO.cookingTimerMax) {
                            GetKitchenObject().DestroySelf();
                    
                            KitchenObject.CreateKitchenObject(cookingRecipeSO.output, this);
                            
                            currentState = State.Cooked;
                        }
                    }
                    break;
                case State.Cooked:
                    cookingProcess += Time.deltaTime;
                    if(cookingRecipeSO != null) {
                        if(cookingProcess >= cookingRecipeSO.cookingTimerMax) {
                            GetKitchenObject().DestroySelf();
                    
                            KitchenObject.CreateKitchenObject(cookingRecipeSO.output, this);
                            
                            currentState = State.Cooked;
                        }
                    }
                    break;
                case State.Burned:
                    break;
                default:
                    break;
            }  
        }
    }
    public override void Interact(Player player)
    {
        if(!HasKitchenObject()) {
            if(player.HasKitchenObject()) {
                if(HasRecipeWithOutput(player.GetKitchenObject().GetKitchenObjectSO())){
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    currentState = State.Cooking;
                    cookingRecipeSO = GetCookingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
                    cookingProcess = 0f;
                }
            }
        } else {
            if(!player.HasKitchenObject()) {
                GetKitchenObject().SetKitchenObjectParent(player);
                cookingProcess = 0f;
            }
        }
    }

    private bool HasRecipeWithOutput(KitchenObjectSO kitchenObjectSO) {
        foreach (CookingRecipeSO recipeSO in cookingRecipes) {
            if(recipeSO.input == kitchenObjectSO) {
                return true;
            }
        }
        return false;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSO) {
        foreach (CookingRecipeSO recipeSO in cookingRecipes) {
            if(recipeSO.input == kitchenObjectSO) {
                return recipeSO.output;
            }
        }
        return null;
    }

    private CookingRecipeSO GetCookingRecipeWithInput(KitchenObjectSO kitchenObjectSO) {
        foreach (CookingRecipeSO recipeSO in cookingRecipes) {
            if(recipeSO.input == kitchenObjectSO) {
                return recipeSO;
            }
        }
        return null;
    }
}
