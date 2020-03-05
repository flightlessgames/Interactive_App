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
    [SerializeField] private Text title = null, price = null;
    [SerializeField] private Image itemImage = null;

    [Header("Settings")]
    [SerializeField] private int _sellFoundation = 10;
    [SerializeField] private Sprite _soldOut = null;

    private int _currentSold = 0;
    private int _sellLimit = 0;

    public bool CanSell
    {
        get
        {
            return (_currentSold < _sellLimit);
        }
    }

    private Ingredients_sObj selfIngredient; 
    public Ingredients_sObj Ingredient { get { return selfIngredient; } }

    [SerializeField] private ShopFunctionController _shopController = null;

    //TODO see ShopUI script, can't pass current game object (panel/button/slot UI group that this script is attached to) into ShopUI current selection script
    public void OnClick()
    {
        Debug.Log(gameObject);

        if (_currentSold < _sellLimit)
        {
            _shopController.SelectItem(this);
        }
    }

    public void SetIngredient(Ingredients_sObj ingredient)
    {
        _currentSold = 0;

        title.text = ingredient.Name;
        price.text = ingredient.Cost.ToString();
        itemImage.sprite = ingredient.Image;
        selfIngredient = ingredient;

        _sellLimit = _sellFoundation - ingredient.Cost;
    }

    public void BoughtItem()
    {
        _currentSold++;
        if(_currentSold >= _sellLimit)
        {
            itemImage.sprite = _soldOut;
        }
    }
}

