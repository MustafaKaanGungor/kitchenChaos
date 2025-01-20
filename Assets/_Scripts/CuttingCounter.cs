using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public event EventHandler<OnProgressChangedEventArgs> OnProcessChanged;
    public class OnProgressChangedEventArgs : EventArgs {
        public float progressNormalized;
    }

    [SerializeField] private CuttingRecipeSO[] cuttingRecipes;

    private int cuttingProcess;
    private int cutAmount;
    [SerializeField] private Animator animator;
    public override void Interact(Player player)
    {
        if(!HasKitchenObject()) {
            if(player.HasKitchenObject()) {
                if(HasRecipeWithOutput(player.GetKitchenObject().GetKitchenObjectSO())){
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProcess = 0;

                    OnProcessChanged?.Invoke(this, new OnProgressChangedEventArgs() {
                        progressNormalized = (float) cuttingProcess/GetCuttingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()).cuttingAmount
                    });
                }
            }
        } else {
            if(!player.HasKitchenObject()) {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void AltInteract(Player player)
    {
        if(HasKitchenObject()) {
            KitchenObjectSO kitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            if(kitchenObjectSO == null) {
                return;
            }
            cuttingProcess++;
            animator.SetTrigger("Cut");
            OnProcessChanged?.Invoke(this, new OnProgressChangedEventArgs() {
                progressNormalized = (float) cuttingProcess/GetCuttingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()).cuttingAmount
            });
            CuttingRecipeSO cuttingRecipe = GetCuttingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
            if(cuttingRecipe != null)
            if(cuttingProcess >= cuttingRecipe.cuttingAmount) {
                GetKitchenObject().DestroySelf();
            
                KitchenObject.CreateKitchenObject(kitchenObjectSO, this);
            }
        }
    }

    private bool HasRecipeWithOutput(KitchenObjectSO kitchenObjectSO) {
        foreach (CuttingRecipeSO recipeSO in cuttingRecipes) {
            if(recipeSO.input == kitchenObjectSO) {
                return true;
            }
        }
        return false;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSO) {
        foreach (CuttingRecipeSO recipeSO in cuttingRecipes) {
            if(recipeSO.input == kitchenObjectSO) {
                return recipeSO.output;
            }
        }
        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeWithInput(KitchenObjectSO kitchenObjectSO) {
        foreach (CuttingRecipeSO recipeSO in cuttingRecipes) {
            if(recipeSO.input == kitchenObjectSO) {
                return recipeSO;
            }
        }
        return null;
    }
}
