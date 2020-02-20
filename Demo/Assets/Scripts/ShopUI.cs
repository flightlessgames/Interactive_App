using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopUI : MonoBehaviour
{
    private List<ShopItem> shopInventory;

    public GridLayoutGroup shopLayout;
    [SerializeField] Button confirmPurchaseBttn;

    [SerializeField] int shopInventoryItemsToSpawnIndex = 0;
    [SerializeField] Shop shopReference;

    public List<ShopSlot> buySlots;
    public int buySlotsIndex;

    public GameObject currentShopSlot;       //TODO always returning null, can't get reference so I can't clear this slot after buying
    public Ingredients_sObj currIngredientSelection;
    
    public Button purchaseButton;

    public Gold currGold;


    private void Awake()
    {
        shopInventory = shopReference.shopInventory;
        fillSlots();

    }

    //TODO: timer-based refresh
    public void fillSlots()
    {
        foreach (ShopSlot shopSlot in buySlots) {
            shopInventoryItemsToSpawnIndex = UnityEngine.Random.Range(0, shopReference.shopInventory.Count);

           /* Debug.Log("my item idex is: " + shopInventoryItemsToSpawnIndex);
            Debug.Log("my item is " + shopReference.shopInventory[shopInventoryItemsToSpawnIndex].item);*/

            shopSlot.title.text = shopReference.shopInventory[shopInventoryItemsToSpawnIndex].item.Name;
            shopSlot.price.text = shopReference.shopInventory[shopInventoryItemsToSpawnIndex].item.Cost.ToString();
            shopSlot.itemImage.sprite = shopReference.shopInventory[shopInventoryItemsToSpawnIndex].item.Image;
            shopSlot.selfIngredientIndex = shopInventoryItemsToSpawnIndex;
            shopSlot.selfIngredient = shopReference.shopInventory[shopInventoryItemsToSpawnIndex].item;
        }
        
    }

    public void debugValues() 
    {
    
    }
    public void OnClick()
    {
        Debug.Log("clicked");
        buyItem();
    }
    public void buyItem() 
    {
        Debug.Log("attempting to buy" + currIngredientSelection);

        if (currIngredientSelection != null) 
        {
            if (currGold.currentGold >= currIngredientSelection.Cost) {
                Debug.Log("bought " + currIngredientSelection);
                currIngredientSelection.IncreaseQuantity(1);

                currGold.currentGold -= currIngredientSelection.Cost;
            }
            
        }
    }

    
}
