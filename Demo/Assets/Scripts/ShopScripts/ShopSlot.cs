using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable] 
[RequireComponent(typeof(Button))]
public class ShopSlot : MonoBehaviour
{
    //TODO reference whole UI, add button reference to that UI, fill button here
    [SerializeField] private Text title, price;
    [SerializeField] private Image itemImage;

    private Ingredients_sObj selfIngredient; 

    [SerializeField] private ShopFunctionController _shopController;

    //TODO see ShopUI script, can't pass current game object (panel/button/slot UI group that this script is attached to) into ShopUI current selection script
    public void OnClick()
    {
        Debug.Log(gameObject);

        _shopController.SelectItem(selfIngredient);
    }

    public void InitializeData(Ingredients_sObj ingredient)
    {
        title.text = ingredient.Name;
        price.text = ingredient.Cost.ToString();
        itemImage.sprite = ingredient.Image;
        selfIngredient = ingredient;
    }

}

