using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable] 
[RequireComponent(typeof(Button))]
public class ShopSlot : MonoBehaviour
{
    [Header("Required")]
    //TODO reference whole UI, add button reference to that UI, fill button here
    [SerializeField] private Text title, price;
    [SerializeField] private Image itemImage;

    [Header("Settings")]
    [SerializeField] private int _sellFoundation = 10;
    [SerializeField] private Sprite _soldOut = null;

    private int _sellableLimit = 0;
    public bool CanSell { get
        {
            return (_sellableLimit > 0);
        } }

    private Ingredients_sObj selfIngredient; 
    public Ingredients_sObj Ingredient { get { return selfIngredient; } }

    [SerializeField] private ShopFunctionController _shopController;

    //TODO see ShopUI script, can't pass current game object (panel/button/slot UI group that this script is attached to) into ShopUI current selection script
    public void OnClick()
    {
        Debug.Log(gameObject);

        if (_sellableLimit > 0)
        {
            _shopController.SelectItem(this);
        }
    }

    public void SetIngredient(Ingredients_sObj ingredient)
    {
        title.text = ingredient.Name;
        price.text = ingredient.Cost.ToString();
        itemImage.sprite = ingredient.Image;
        selfIngredient = ingredient;

        _sellableLimit = _sellFoundation - ingredient.Cost;
    }

    public void BoughtItem()
    {
        _sellableLimit--;
        if(_sellableLimit < 1)
        {
            itemImage.sprite = _soldOut;
        }
    }
}

