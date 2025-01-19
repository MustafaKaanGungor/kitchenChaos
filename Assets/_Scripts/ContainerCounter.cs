using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO tomatoSO;

    public override void Interact(Player player) {
        if(HasKitchenObject()) {
            Transform kitchenObjectTransform = Instantiate(tomatoSO.prefab);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
        }
    }

}
