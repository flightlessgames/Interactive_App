using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PageDisplay : MonoBehaviour
{
    [SerializeField] private Ingredients_sObj _ingredient = null;

    [SerializeField] private Text _ingredientName = null;
    [SerializeField] private Text _ingredientDescription = null;
    [SerializeField] private Image _ingredientImage = null;

    [SerializeField] private PotionHistoryController _potionHistory = null;

    public void SetIngredient (Ingredients_sObj inputIngredient)
    {
        _ingredient = inputIngredient;

        Display();
    }

    private void Display()
    {
        _ingredientName.text = _ingredient.Name;
        _ingredientDescription.text = _ingredient.Description;
        _ingredientImage.sprite = _ingredient.Image;

        _potionHistory.SetIngredientHistory(_ingredient);
    }


}
