using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(displayIngredient))]

//we're requiring type of Button, but not using in the script. Use Designer/Component interface to link the Button's OnClick to this script's OnClick();
[RequireComponent(typeof(Button))]
public class craftingSlotController : MonoBehaviour
{
    //initialise with none/null, displayIngredient is Empty.
    private displayIngredient _ingredient = null;

    //because displayIngredient is the _emptyIngredient_ingred object, we're saving that locally to re-use instead of null values
    private Ingredients_sObj _nullIngredient = null;

    [SerializeField] CraftingUIController _craftingController = null;

    private void Awake()
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
        Debug.Log("Set slot to Ingredient being held: " + _craftingController.CurrIngredient);

        //to set our ingredient's data, we tell the displayIngredient object to update /its/ data to the new _ingred
        _ingredient.SetIngredient(_craftingController.CurrIngredient);

        //after using an ingredient ONCE we tell the controller to stop using it, and DROP that _ingred
        //possible to remove this clause? let the player multi-set ingredients. Might add a _null to the HotBar if we do.
        _craftingController.DropIngredient();
    }

    //public to clearIngredients after each craft using _devCrafting script
    public void ClearIngredients()
    {
        _ingredient.SetIngredient(_nullIngredient);
    }
}
