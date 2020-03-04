using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateButton : MonoBehaviour
{
    // Connect to shopping inventory, button prefab, foreach ing create new button w/ ing information, make sure button prefab connects PageDispay

    [SerializeField] private List<Ingredients_sObj> defaultIngredients = new List<Ingredients_sObj>();
    [SerializeField] private GameObject buttonPrefab = null;
    [SerializeField] private PageDisplay _pageDisplay = null;

    private List<Ingredients_sObj> allIngredients = new List<Ingredients_sObj>();

    private void Start()
    {
        //waiting for StateController to run function in Awake()
        allIngredients.AddRange(defaultIngredients);
        allIngredients.AddRange(fileUtility.Shop.Inventory);

        CreateButtons();
    }

    private void CreateButtons()
    {
        foreach(Ingredients_sObj ing in allIngredients)
        {
            //prefab of button, with known DisplayIngredient data and JournalButton script
            GameObject prefab = Instantiate(buttonPrefab, transform);

            //prefab also has JournalButtonPress which needs a pageDisplay reference to actually do its job of updating the page's display

            //displayIngredient is not IN prefab, its nested in a child, but is reachable through jButton functions.
            JournalButtonPress jButton = prefab.GetComponent<JournalButtonPress>();
            jButton.SetPageDisplay(_pageDisplay);
            jButton.SetIngredient(ing);
        }
    }
}
