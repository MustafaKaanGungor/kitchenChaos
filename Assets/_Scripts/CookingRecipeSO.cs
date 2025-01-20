using UnityEngine;

[CreateAssetMenu(fileName = "CookingRecipeSO", menuName = "CookingRecipeSO", order = 0)]
public class CookingRecipeSO : ScriptableObject {
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float cookingTimerMax;
}