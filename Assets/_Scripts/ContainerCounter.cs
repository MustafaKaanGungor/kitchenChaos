using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] Animator animator;

    public override void Interact(Player player) {
        if(!HasKitchenObject() && !player.HasKitchenObject()) {
            KitchenObject.CreateKitchenObject(kitchenObjectSO, player);
            animator.SetTrigger("OpenClose");
        }
    }

}
