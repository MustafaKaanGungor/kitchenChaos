using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player) {
        if(!HasKitchenObject() && !player.HasKitchenObject()) {
            KitchenObject.CreateKitchenObject(kitchenObjectSO, player);
        }
    }

}
