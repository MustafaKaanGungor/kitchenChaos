using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] Animator animator;

    public override void Interact(Player player) {
        if(!HasKitchenObject() && !player.HasKitchenObject()) {
            KitchenObject.CreateKitchenObject(kitchenObjectSO, player);
            animator.SetTrigger("OpenClose");
            InteractServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractServerRpc() {
        InteractClientRpc();
    }

    [ClientRpc]
    private void InteractClientRpc() {
        animator.SetTrigger("OpenClose");
    }
 
}
