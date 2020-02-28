using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PageDisplay : MonoBehaviour
{
    [SerializeField] private Ingredients_sObj _ingredient;

    [SerializeField] private Text pageTitle;
    [SerializeField] private Text pageDescription;
    [SerializeField] private Image pageIcon;

    [SerializeField] private List<Ingredients_sObj> _defaults;
    [SerializeField] private Shop _ShopList;
    private List<Ingredients_sObj> _allIngredients = new List<Ingredients_sObj>();

    private void Awake()
    {
        _allIngredients.AddRange(_defaults);
        _allIngredients.AddRange(_ShopList.Inventory);
        _ingredient = _allIngredients[0];
    }

    private void Start()
    {
        Display();
    }

    public void setPage (int index)
    {
        _ingredient = _allIngredients[index];
        Debug.Log(_ingredient.Name);
        Display();

    }

    void Display()
    {
        pageTitle.text = _ingredient.Name;
        pageDescription.text = _ingredient.Description;
        pageIcon.sprite = _ingredient.Image;
    }


}
