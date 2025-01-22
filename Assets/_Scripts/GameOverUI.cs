using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deliveryText;

    private void Start() {
        GameManager.Instance.OnStateChanged += GamemanagerOnStateChanged;
    
        Hide();
    }

    private void GamemanagerOnStateChanged(object sender, EventArgs e)
    {
        if(GameManager.Instance.IsGameOver()) {
            Show();
            deliveryText.text = DeliveryManager.Instance.GetSuccessfulDeliveries().ToString();
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    } 
}
