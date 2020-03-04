using UnityEngine;

[CreateAssetMenu(fileName = "New_Potion_sObj", menuName = "sObj/Potion")]
public class Potion_sObj : ScriptableObject
{
    public Vector3 Recipe = Vector3.zero;
    public string Name = "...";
    public Sprite Image = null;
}
