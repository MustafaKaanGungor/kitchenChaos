using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;
    
    private void Awake() {
        resumeButton.onClick.AddListener(() => {
            GameManager.Instance.ToggleGamePause();
        });
        optionsButton.onClick.AddListener(() => {
            OptionsUI.Instance.Show(Show);
            Hide();
        });
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scenes.MainMenuScene);
        });
    }
    private void Start() {
        GameManager.Instance.OnGamePaused += GameManagerOnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManagerOnGameUnpaused;
        Hide();
    }

    private void GameManagerOnGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void GameManagerOnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void Show() {
        gameObject.SetActive(true);
        resumeButton.Select();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
