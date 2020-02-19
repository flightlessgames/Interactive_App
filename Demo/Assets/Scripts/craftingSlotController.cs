using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(displayIngredient))]
public class craftingSlotController : Selectable    //by using the Selectable parent object, we inherit the properties of a button.
{
    //initialise with none/null, displayIngredient is Empty.
    private displayIngredient _ingredient = null;

    //because displayIngredient is the _emptyIngredient_ingred object, we're saving that locally to re-use instead of null values
    private Ingredients_sObj _nullIngredient = null;

    [SerializeField] CraftingUIController _craftingController = null;
    [SerializeField] private Text _debugText = null;

    override protected void Awake()
    {
        _ingredient = GetComponent<displayIngredient>();
        _nullIngredient = _ingredient.IngredientData;
    }

    //when crafting, pull my data (find what my display ingredient is) and return my score vector.
    public Vector3 ScoreIngredient()
    {
        return _ingredient.IngredientData.Values;
    }

    //using a generic OnClick() function to link the Button component's commands to this script. Useful for Computer & Touch Devices.
    public void OnClick()
    {
        Debug.Log("OnClicked()");

        if (!_craftingController.CurrIngredient)
        {
            //if there is no ingredient held, assume player is trying to ERASE a slot value, then set to prescribed null value.
            Debug.Log("No Ingredient Held");
            ClearIngredients();
            return;
        }

        //if there IS an ingredient held by the controller, then apply that ingredient to the slot, then DROP that ingredient.
        //to set our ingredient's data, we tell the displayIngredient object to update /its/ data to the new _ingred
        _ingredient.SetIngredient(_craftingController.CurrIngredient);
    }

    //this is roughly the OnClick() functionality of a Button
    override public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("OnSelect");
        OnClick();
    }

    //this detects player input as HOVER or HIGHLIGHT, useful for drag/swirl crafting
    override public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        _debugText.text += "\nOnPointerEnter()";
        OnClick();
    }

    //public to clearIngredients after each craft using _devCrafting script
    public void ClearIngredients()
    {
        _ingredient.SetIngredient(_nullIngredient);
    }
}
