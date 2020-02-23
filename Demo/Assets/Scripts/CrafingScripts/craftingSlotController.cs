using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class craftingSlotController : Selectable    //by using the Selectable parent object, we inherit the properties of a button.
{
    //initialise with none/null, displayIngredient is Empty.
    [SerializeField] private displayIngredient _slotIngredient = null;
    [SerializeField] CraftingUIController _craftingController = null;

    //because displayIngredient is the _emptyIngredient_ingred object, we're saving that locally to re-use instead of null values
    private Ingredients_sObj _nullIngredient = null;
    public Ingredients_sObj Ingredient
    {
        get
        {
            return _slotIngredient.IngredientData;
        }
    }
    
    

    override protected void Awake()
    {
        _nullIngredient = _slotIngredient.IngredientData;
    }

    //when crafting, pull my data (find what my display ingredient is) and return my score vector.
    public Vector3 ScoreIngredient()
    {
        return _slotIngredient.IngredientData.Values;
    }

    //using a generic OnClick() function to link the Button component's commands to this script. Useful for Computer & Touch Devices.
    public void OnClick()
    {
        Debug.Log("OnClicked()");

        //if our current ingredient IS the ingredient held, do NOTHING
        if (_slotIngredient.IngredientData == _craftingController.CurrIngredient)
            return;

        //otherwise (our ingredient is different than _crafting.controller's, make the swap
        Ingredients_sObj heldIngredient = _craftingController.CurrIngredient;

        if (heldIngredient.Quantity != 0)    //>0 would exclude our -1 values, which are our Infinite values
        {
            //give one back to previous "current" ingredient,
            _slotIngredient.IngredientData.IncreaseQuantity(1);

            //set our ingredient to the new Ingredient_sObj, then take 1 away
            _slotIngredient.SetIngredient(heldIngredient);
            heldIngredient.DecreaseQuantity(1);
        }
        else
        {
            //if our ingredient IS == 0, we have run out of a natural ingredient type
            //we force the _craftingController to "drop" the ingredient and return to NULL
            _craftingController.HoldIngredient(_nullIngredient);
            Debug.Log("Cannot use " + heldIngredient.Name);
        }
        
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
        //_debugText.text += "\nOnPointerEnter()";
        OnClick();
    }

    //public to clearIngredients after each craft using _devCrafting script
    public void ClearIngredients()
    {
        _slotIngredient.SetIngredient(_nullIngredient);

        //this line of code is run up to 5 times through _devCrafting list of ALL craftingSlotControllers,
        //dont currently reference the _craftingController through _devCrafting, so we're leaving it like this until we do
        _craftingController.HoldIngredient(_nullIngredient);
    }
}
