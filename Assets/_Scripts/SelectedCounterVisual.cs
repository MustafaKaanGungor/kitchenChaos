using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject selectedCounterVisual;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += InstanceOnSelectedCounterChanged;
    }

    void InstanceOnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e) {
        if(e.selectedCounter == clearCounter) {
            selectedCounterVisual.SetActive(true);
        } else {
            selectedCounterVisual.SetActive(false);
        }
    }
}
