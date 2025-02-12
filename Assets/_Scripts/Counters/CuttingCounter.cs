using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public static event EventHandler OnAnyCut;
    new public static void ResetStaticData() {
        OnAnyCut = null;
    }
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
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())){
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);
                    
                    InteractLogicPlaceObjectOnCounterServerRpc();
                }
            }
        } else {
            if(!player.HasKitchenObject()) {
                GetKitchenObject().SetKitchenObjectParent(player);
            } else {
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    }
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)] 
    private void InteractLogicPlaceObjectOnCounterServerRpc() {
        InteractLogicPlaceObjectOnCounterClientRpc();
    }

    [ClientRpc] 
    private void InteractLogicPlaceObjectOnCounterClientRpc() {
        cuttingProcess = 0;

        OnProcessChanged?.Invoke(this, new OnProgressChangedEventArgs() {
            progressNormalized = 0f
        });
    }

    public override void AltInteract(Player player)
    {
        if(HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            CutObjectServerRpc();
            TestCuttingProgressDoneServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)] 
    private void CutObjectServerRpc() {
        if(HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            CutObjectClientRpc();
        }
    }

    [ClientRpc] 
    private void CutObjectClientRpc() {
        KitchenObjectSO kitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
        if(kitchenObjectSO == null) {
            return;
        }
        cuttingProcess++;
        animator.SetTrigger("Cut");
        OnAnyCut?.Invoke(this, EventArgs.Empty);
        OnProcessChanged?.Invoke(this, new OnProgressChangedEventArgs() {
            progressNormalized = (float) cuttingProcess/GetCuttingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()).cuttingAmount
        });
    }

    [ServerRpc(RequireOwnership = false)]
    private void TestCuttingProgressDoneServerRpc() {
        CuttingRecipeSO cuttingRecipe = GetCuttingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
        if(cuttingRecipe != null)
        if(cuttingProcess >= cuttingRecipe.cuttingAmount) {
            KitchenObject.DestroyKitchenObject(GetKitchenObject());
            KitchenObjectSO kitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            KitchenObject.CreateKitchenObject(kitchenObjectSO, this);
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeWithInput(kitchenObjectSO);
        return cuttingRecipeSO != null;
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
