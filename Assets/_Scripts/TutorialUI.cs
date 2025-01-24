using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI key1;
    [SerializeField] private TextMeshProUGUI key2;
    [SerializeField] private TextMeshProUGUI key3;
    [SerializeField] private TextMeshProUGUI key4;
    [SerializeField] private TextMeshProUGUI key5;
    [SerializeField] private TextMeshProUGUI key6;
    [SerializeField] private TextMeshProUGUI key7;

    private void Start() {
        GameInput.Instance.OnBindingRebind += GameInputOnBindingRebind;
        GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;
        UpdateVisuals();
        Show();
    }

    private void GameManagerOnStateChanged(object sender, EventArgs e)
    {  
        if(GameManager.Instance.IsCountdownActive()) {
            Hide();
        }
    }

    private void GameInputOnBindingRebind(object sender, EventArgs e)
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        key1.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        key2.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        key3.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        key4.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        key5.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        key6.text = GameInput.Instance.GetBindingText(GameInput.Binding.AltInteract);
        key7.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);

    }
    
    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    } 
}
