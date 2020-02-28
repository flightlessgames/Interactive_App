using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopFunctionController : MonoBehaviour
{
    [SerializeField] private List<ShopSlot> _purchaseSlots;
    [SerializeField] Text _feedbackText = null;

    private List<Ingredients_sObj> _rareIngredients = new List<Ingredients_sObj>();
    private List<Ingredients_sObj> _uncomIngredients = new List<Ingredients_sObj>();
    private List<Ingredients_sObj> _commonIngredients = new List<Ingredients_sObj>();

    private List<ShopSlot> _rareSlots = new List<ShopSlot>();
    private List<ShopSlot> _uncomSlots = new List<ShopSlot>();
    private List<ShopSlot> _commonSlots = new List<ShopSlot>();

    private ShopSlot _recentSlot = null;

    public void RandomizeShop()
    {
        Debug.Log("Randomizing");
        RariftSlots();
        RarifyIngredients();

        fillSlots();
    }

    private void RariftSlots()
    {
        int countSlots = _purchaseSlots.Count;

        int countCommon = countSlots / 2;   //half of all slots are COMMON (1/2 of Total)   //3
        int countRare = (countSlots - countCommon) / 3; //one third of remaining slots are RARE (1/6 of Total)  //1
        int countUncommon = countSlots - (countCommon + countRare); //uncommons are the remains (2/6 or 1/3 of Total)   //2

        for(int i=0; i<_purchaseSlots.Count; i++)
        {
            //if I [0] is less than rare 1
            if (i < countRare)
            {
                _rareSlots.Add(_purchaseSlots[i]);
            }

            //if I [1][2] is less than rare+uncommon 3
            else if (i < countRare + countUncommon)
            {
                _uncomSlots.Add(_purchaseSlots[i]);
            }

            //if I [3][4][5] is remainder
            else
            {
                _commonSlots.Add(_purchaseSlots[i]);
            }
        }
    }

    private void RarifyIngredients()
    {
        foreach(Ingredients_sObj ingred in fileUtility._shop.Inventory)
        {
            //rare ingredients should push scores towards the corners, maximum magnitude
            //rare scores are (2,1,-2) or (2,-1,-2) etc
            //and (2,2,2) CBD Oil
            if(ingred.Cost >= 5)
            {
                _rareIngredients.Add(ingred);
            }

            //uncommon ingredients are non-minimal scores, average magnitude
            //uncommon scores are (2,0,-2) or (2,1,-1) etc
            else if(ingred.Cost == 4)
            {
                _uncomIngredients.Add(ingred);
            }

            //common ingredients are near 0 values, smallest magnitude
            else
            {
                _commonIngredients.Add(ingred);
            }
        }
    }

    //TODO Timer Refresh?
    public void fillSlots()
    {
        AssignRarity(_rareSlots, _rareIngredients);
        AssignRarity(_uncomSlots, _uncomIngredients);
        AssignRarity(_commonSlots, _commonIngredients);
    }

    private void AssignRarity(List<ShopSlot> SlotList, List<Ingredients_sObj> IngredList)
    {
        foreach (ShopSlot slot in SlotList)
        {
            int shopIndex = UnityEngine.Random.Range(0, IngredList.Count); //INCLUSIVE random among IngredientList

            if (shopIndex == IngredList.Count)
                shopIndex--; //cannot be EQUALS to COUNT, as COUNT13 cannot accept ingredient[13]

            Debug.Log("random item is: " + shopIndex);

            slot.SetIngredient(IngredList[shopIndex]);
        }
    }

    public void SelectItem(ShopSlot slot)
    {
        _recentSlot = slot;
        _feedbackText.text = "You are trying to buy a: " + _recentSlot.Ingredient.Name;
    }
    
    public void BuyItem() 
    {
        if (_recentSlot.Ingredient != null) 
        {
            if (!_recentSlot.CanSell)
                return;

            if (fileUtility.SaveObject.gold >= _recentSlot.Ingredient.Cost)
            {
                _recentSlot.Ingredient.IncreaseQuantity(1);

                fileUtility.SaveObject.gold -= _recentSlot.Ingredient.Cost;

                _feedbackText.text = "BOUGHT: " + _recentSlot.Ingredient.Name + 
                    "\nYou have: " + fileUtility.SaveObject.gold + " gold" + 
                    "\nYou have: " + _recentSlot.Ingredient.Quantity + " " + _recentSlot.Ingredient.Name;

                _recentSlot.BoughtItem();
            }
            else
            {
                _feedbackText.text = "Cannot afford a " + _recentSlot.Ingredient.Name + "," +
                    "\nYou have: " + fileUtility.SaveObject.gold + " gold" +
                    "\nYou need: " + _recentSlot.Ingredient.Cost + " gold";
            }
        }
    }
}
