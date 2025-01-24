using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;


    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private float plateSpawnTimer;
    [SerializeField] private float plateSpawnTime = 4f;
    private int plateSpawnAmount;
    private int plateSpawnAmountMax = 4;

    private void Update() {
        plateSpawnTimer += Time.deltaTime;
        if(plateSpawnTimer >= plateSpawnTime) {
            plateSpawnTimer = 0f;

            if(GameManager.Instance.IsGamePlaying() && plateSpawnAmount < plateSpawnAmountMax) {
                plateSpawnAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        
        }
    }

    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject()) {
            if(plateSpawnAmount > 0) {
                KitchenObject.CreateKitchenObject(kitchenObjectSO, player);
                plateSpawnAmount--;
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
