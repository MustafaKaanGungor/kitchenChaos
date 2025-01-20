using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image progressbar;

    private void Start() {
        cuttingCounter.OnProcessChanged += CuttingCounter_OnProcessChanged;
        progressbar.fillAmount = 0f;
        Hide();
    }

    private void CuttingCounter_OnProcessChanged(object sender, CuttingCounter.OnProgressChangedEventArgs e)
    {
        progressbar.fillAmount = e.progressNormalized; 
        if(progressbar.fillAmount == 0f || progressbar.fillAmount == 1f) {
            Hide();
        } else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
