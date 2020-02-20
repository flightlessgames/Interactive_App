using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable] 
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(displayIngredient))]
public class ShopSlot : MonoBehaviour
{
    //TODO reference whole UI, add button reference to that UI, fill button here
    public Text title, price;
    public Button purchaseButton;       //forgot why I put this here, not deleting just in case
    public Image itemImage;
    public int selfIngredientIndex;
    public ShopSlot selfIngredientsObj;

    public Ingredients_sObj selfIngredient; 

    [SerializeField] ShopUI slotSelectionController;

    //TODO see ShopUI script, can't pass current game object (panel/button/slot UI group that this script is attached to) into ShopUI current selection script
    public void OnClick()
    {
        Debug.Log(gameObject);
        Debug.Log(selfIngredientIndex);

        Debug.Log(selfIngredientsObj);
        Debug.Log(selfIngredient);

        slotSelectionController.currIngredientSelection = selfIngredient;

        Debug.Log("thing " + slotSelectionController.currIngredientSelection);
    }

}

