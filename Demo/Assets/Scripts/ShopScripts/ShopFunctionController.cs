using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopFunctionController : MonoBehaviour
{
    [SerializeField] Shop _shopReference;
    [SerializeField] private List<ShopSlot> _purchaseSlots;
    [SerializeField] Text _feedbackText = null;

    private List<Ingredients_sObj> _shopInventory;

    public Ingredients_sObj _currIngredient;

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

    public void SelectItem(Ingredients_sObj item)
    {
        _currIngredient = item;
        _feedbackText.text = "You are trying to buy a: " + _currIngredient.Name;
    }
    
    public void BuyItem() 
    {
        Debug.Log("attempting to buy" + _currIngredient);

        if (_currIngredient != null) 
        {
            if (fileUtility.SaveObject.gold >= _currIngredient.Cost) {

                Debug.Log("bought " + _currIngredient);
                _currIngredient.IncreaseQuantity(1);

                fileUtility.SaveObject.gold -= _currIngredient.Cost;

                _feedbackText.text = "BOUGHT: " + _currIngredient.Name + 
                    "\nYou have: " + fileUtility.SaveObject.gold + " gold" + 
                    "\nYou have: " + _currIngredient.Quantity + " " + _currIngredient.Name;
            }
            else
            {
                Debug.Log("cannot buy, not enough gold");

                _feedbackText.text = "Cannot afford a " + _currIngredient.Name + "," +
                    "\nYou have: " + fileUtility.SaveObject.gold + " gold" +
                    "\nYou need: " + _currIngredient.Cost + " gold";

                _currIngredient = null;
            }
        }
    }
}
