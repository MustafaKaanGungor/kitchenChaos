using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance {get; private set;}
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button altInteractButton;
    [SerializeField] private Button pauseButton;

    [SerializeField] private TextMeshProUGUI soundEffectText;
    [SerializeField] private TextMeshProUGUI MusicText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI altInteractText;
    [SerializeField] private TextMeshProUGUI pauseText;
    
    [SerializeField] private GameObject rebindScreen;

    private Action onCloseButtonAction;

    private void Awake() {
        Instance = this;
        soundEffectsButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() => {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() => {
            Hide();
            onCloseButtonAction();
        });

        moveUpButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Up);
        });
        moveDownButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Down);
        });
        moveLeftButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Left);
        });
        moveRightButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Right);
        });
        interactButton.onClick.AddListener(() => {  
            RebindBinding(GameInput.Binding.Interact);
        });
        altInteractButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.AltInteract);
        });
        pauseButton.onClick.AddListener(() => { 
            RebindBinding(GameInput.Binding.Pause);
        });
    }

    private void Start() {
        GameManager.Instance.OnGameUnpaused += GameManagerOnGameUnpaused;
        UpdateVisual();
        HidePressToRebindKey();
        Hide();
    }

    private void GameManagerOnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual() {
        soundEffectText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f).ToString();
        MusicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10).ToString();

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        altInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.AltInteract);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    private void RebindBinding(GameInput.Binding binding) {
        ShowPressToRebindKey();
        GameInput.Instance.Rebind(binding, () => {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }

    public void Show(Action onCloseButtonAction) {
        this.onCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);
        soundEffectsButton.Select();    
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey() {
        rebindScreen.SetActive(true);
    }

    private void HidePressToRebindKey() {
        rebindScreen.SetActive(false);
    }
}
