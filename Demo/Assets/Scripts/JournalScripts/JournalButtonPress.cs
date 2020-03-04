using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalButtonPress : MonoBehaviour
{
    [SerializeField] private PageDisplay _pageDisplay;
    [SerializeField] private displayIngredient display = null;

    public void OnClick()
    {
        _pageDisplay.SetIngredient(display.IngredientData);
    }

    public void SetPageDisplay(PageDisplay newPageDisplay)
    {
        _pageDisplay = newPageDisplay;
    }

    public void SetIngredient(Ingredients_sObj ingred)
    {
        display.SetIngredient(ingred);
    }
}
