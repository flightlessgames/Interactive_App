using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopFunctionController : MonoBehaviour
{
    [SerializeField] Shop _shopReference;
    [SerializeField] private List<ShopSlot> _purchaseSlots;
    [SerializeField] private Gold _gold_sObj;

    private List<Ingredients_sObj> _shopInventory;

    public Ingredients_sObj CurrIngredient;

    private void Awake()
    {
        _shopInventory = _shopReference.shopInventory;
        fillSlots();
    }

    //TODO: timer-based refresh
    public void fillSlots()
    {
        foreach (ShopSlot shopSlot in _purchaseSlots) {
            int shopIndex = UnityEngine.Random.Range(0, _shopReference.shopInventory.Count);

            Debug.Log(_shopReference.shopInventory[shopIndex].Name);
            shopSlot.InitializeData(_shopReference.shopInventory[shopIndex]);
        }
    }
    
    public void buyItem() 
    {
        Debug.Log("attempting to buy" + CurrIngredient);

        if (CurrIngredient != null) 
        {
            if (_gold_sObj.currentGold >= CurrIngredient.Cost) {
                Debug.Log("bought " + CurrIngredient);
                CurrIngredient.IncreaseQuantity(1);

                _gold_sObj.currentGold -= CurrIngredient.Cost;
            }
            else
            {
                Debug.Log("cannot buy, not enough gold");
                CurrIngredient = null;
            }
        }
    }
}
