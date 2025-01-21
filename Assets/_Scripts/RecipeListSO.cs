using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeListSO", menuName = "RecipeListSO", order = 0)]
public class RecipeListSO : ScriptableObject {
    public List<RecipeSO> recipeSOs;
}
