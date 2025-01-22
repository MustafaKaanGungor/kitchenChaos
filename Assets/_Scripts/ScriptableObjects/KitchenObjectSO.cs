using UnityEngine;

[CreateAssetMenu(fileName = "KitchenObjectSO", menuName = "KitchenObjectSO", order = 0)]
public class KitchenObjectSO : ScriptableObject {

    public Transform prefab;
    public Sprite sprite;
    public string objectName;
}