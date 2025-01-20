using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoveCounter : BaseCounter
{

    [SerializeField] private CookingRecipeSO[] cookingRecipes;
    private float cookingProcess = 0f;
    private CookingRecipeSO cookingRecipeSO;
    [SerializeField] GameObject oilEffect;
    [SerializeField] GameObject fireEffect;
    [SerializeField] private Image progressBar;
    [SerializeField] private GameObject UIelement;

    private void Update() {
        if(HasKitchenObject() && cookingRecipeSO != null) {
            cookingProcess += Time.deltaTime;
            progressBar.fillAmount = cookingProcess/cookingRecipeSO.cookingTimerMax;
            CookingEffects(true);
            if(cookingProcess >= cookingRecipeSO.cookingTimerMax) {
                GetKitchenObject().DestroySelf();
                    
                KitchenObject.CreateKitchenObject(cookingRecipeSO.output, this);
                    
                cookingRecipeSO = GetCookingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
                cookingProcess = 0f;
                CookingEffects(false);
            }
        }
    }
    public override void Interact(Player player)
    {
        if(!HasKitchenObject()) {
            if(player.HasKitchenObject()) {
                if(HasRecipeWithOutput(player.GetKitchenObject().GetKitchenObjectSO())){
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cookingRecipeSO = GetCookingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
                    cookingProcess = 0f;
                }
            }
        } else {
            if(!player.HasKitchenObject()) {
                GetKitchenObject().SetKitchenObjectParent(player);
                cookingProcess = 0f;
                CookingEffects(false);
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

    private void CookingEffects(bool state) {
        oilEffect.SetActive(state);
        fireEffect.SetActive(state);
        UIelement.SetActive(state);
    }
}
