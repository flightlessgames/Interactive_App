using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(displayIngredient))]

//we're requiring type of Button, but not using in the script. Use Designer/Component interface to link the Button's OnClick to this script's OnClick();
[RequireComponent(typeof(Button))]
public class craftingHotbarController : MonoBehaviour
{
    //using displayIngredient script to Visualise the Ingredient_sObj data
    private displayIngredient _ingredient = null;

    [SerializeField] CraftingUIController _craftingController = null;

    private void Awake()
    {
        _ingredient = GetComponent<displayIngredient>();
    }
    
    //using a generic OnClick() function to link the Button component's commands to this script. Useful for Computer & Touch Devices.
    public void OnClick()
    {
        Debug.Log("OnClick()");
        _craftingController.HoldIngredient(_ingredient.IngredientData);
    }
}
