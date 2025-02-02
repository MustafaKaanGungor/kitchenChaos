using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter counter;
    [SerializeField] private GameObject[] selectedVisual;

    private void Start() {
        if(Player.LocalInstance != null) {
            Player.LocalInstance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        }
        Player.OnAnyPlayerSpawned += PlayerOnAnyPlayerSpawned;
        
    }

    private void PlayerOnAnyPlayerSpawned(object sender, EventArgs e)
    {
        if(Player.LocalInstance != null) {
            Player.LocalInstance.OnSelectedCounterChanged -= Player_OnSelectedCounterChanged;
            Player.LocalInstance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        }
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e) {
        if(e.selectedCounter == counter) {
            foreach (GameObject visualGameObject in selectedVisual) {
                visualGameObject.SetActive(true);
            }
            
        } else {
            foreach (GameObject visualGameObject in selectedVisual) {
                visualGameObject.SetActive(false);
            }
        }
    }
}
