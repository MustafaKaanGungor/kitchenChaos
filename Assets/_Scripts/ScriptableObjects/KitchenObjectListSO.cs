using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KitchenObjectListSO", menuName = "KitchenObjectListSO", order = 0)]
public class KitchenObjectListSO : ScriptableObject {
    public List<KitchenObjectSO> kitchenObjects;
}