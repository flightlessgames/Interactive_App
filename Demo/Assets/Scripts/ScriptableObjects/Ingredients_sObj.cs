using UnityEngine;

[CreateAssetMenu(fileName ="New_Ingredient_sObj", menuName = "sObj/Ingredient")]
public class Ingredients_sObj : ScriptableObject
{

    public Vector3 Values = Vector3.zero;
    public string Name = "...";
    public Sprite Image = null;
}
