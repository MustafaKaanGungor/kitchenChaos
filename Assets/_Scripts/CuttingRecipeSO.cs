using UnityEngine;

[CreateAssetMenu(fileName = "CuttingRecipeSO", menuName = "CuttingRecipeSO", order = 0)]
public class CuttingRecipeSO : ScriptableObject {
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public int cuttingAmount;
}