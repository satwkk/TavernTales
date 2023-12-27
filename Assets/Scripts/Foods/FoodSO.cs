using UnityEngine;

[CreateAssetMenu(fileName = "Cooking Items", menuName = "Food")]
public class FoodSO : ScriptableObject {
    public string foodID;
    public string foodName;
    public string foodDescription;
}
