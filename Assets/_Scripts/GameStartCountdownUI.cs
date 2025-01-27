using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    private Animator animator;
    private int previousCountdownNumber;
    private const string COUNTDOWN_ANIM_TRIGGER = "NumberPopup";

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        GameManager.Instance.OnStateChanged += GamemanagerOnStateChanged;
    
        Hide();
    }

    private void GamemanagerOnStateChanged(object sender, EventArgs e)
    {
        if(GameManager.Instance.IsCountdownActive()) {
            Show();
        } else {
            Hide();
        }
    }

    private void Update() {
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();
 
        if(countdownNumber != previousCountdownNumber) {
            animator.SetTrigger(COUNTDOWN_ANIM_TRIGGER);
            previousCountdownNumber = countdownNumber;
            SoundManager.Instance.PlayCountdownSound();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
    
}
