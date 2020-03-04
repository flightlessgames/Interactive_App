using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop", menuName = "Shop")]
public class Shop : ScriptableObject
{
    [SerializeField] private List<Ingredients_sObj> _shopInventory = new List<Ingredients_sObj>();
    public List<Ingredients_sObj> Inventory { get { return _shopInventory; } }
}
