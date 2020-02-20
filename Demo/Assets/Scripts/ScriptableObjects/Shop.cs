using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop", menuName = "Shop")]
public class Shop : ScriptableObject
{
    public List<ShopItem> shopInventory;
}
