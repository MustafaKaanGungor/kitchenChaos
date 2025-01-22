using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {get; private set;}
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private void Awake() {
        Instance = this;
    }
    private void Start() {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManagerOnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFail += DeliveryManagerOnRecipeFail;
        CuttingCounter.OnAnyCut += CuttingCounterOnAnyCut;  
        Player.Instance.OnPlayerPick += PlayerOnPlayerPick;
        BaseCounter.OnAnyObjectPlaced += CounterOnAnyObjectPlaced;
        TrashContainer.OnAnyObjectTrashed += TrashContainerOnAnyObjectTrashed;
    }

    private void TrashContainerOnAnyObjectTrashed(object sender, EventArgs e)
    {
        TrashContainer trashContainer = sender as TrashContainer;
        PlaySound(audioClipRefsSO.trash, trashContainer.transform.position, 1);
    }

    private void CounterOnAnyObjectPlaced(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void PlayerOnPlayerPick(object sender, EventArgs e)
    {
        Player player = Player.Instance;
        PlaySound(audioClipRefsSO.objectPickup, player.transform.position, 1);
    }

    private void CuttingCounterOnAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position, 1);
    }

    private void DeliveryManagerOnRecipeFail(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position, 1);
    }

    private void DeliveryManagerOnRecipeSuccess(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position, 1);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
    }

    public void PlayFootstepSound(Vector3 position, float volume = 1f) {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }
}
