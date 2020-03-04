using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalOrVerticalLayoutGroup))]
public class PotionHistoryController : MonoBehaviour
{
    private HorizontalOrVerticalLayoutGroup _layoutGroup = null;

    [Header("Required")]
    [SerializeField] private PotionHistorySlot _slotPrefab = null;
    
    [Header("Optional Settings")]
    [SerializeField] private List<PotionHistorySlot> _histories = new List<PotionHistorySlot>();
    [SerializeField] private bool _needsAdjustment = false;
    [SerializeField] private Ingredients_sObj _ingredientHistory = null;

    private int _ingredientIndex = 0;

    private void Awake()
    {
        _layoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
        
    }

    private void Start()
    {
        SetIngredientHistory(_ingredientHistory);
    }

    public void SetIngredientHistory(Ingredients_sObj ingred)
    {
        _ingredientHistory = ingred;
        CreateHistorySlots();
        UpdateHistories();
    }

    public void UpdateHistories()
    {
        //disable all slots to return to pool
        foreach (PotionHistorySlot slot in _histories)
        {
            slot.gameObject.SetActive(false);
        }

        //if we do NOT have an ingredient to check
        if (!_ingredientHistory)
        {
            //save off all recipes found in recentRecipe List<>

            List<_devCrafting.Recipe> allRecipes = new List<_devCrafting.Recipe>();

            foreach (SaveFile.RecipeList recipeArray in fileUtility.SaveObject.recentRecipes)
            {
                foreach (_devCrafting.Recipe recipe in recipeArray.recipes)
                {
                    foreach (_devCrafting.Recipe savedRecipe in allRecipes)
                    {
                        //do NOT display recipes with the same data
                        if (recipe == savedRecipe) { continue; }
                        allRecipes.Add(recipe);
                    }
                }
            }

            foreach (_devCrafting.Recipe recipe in allRecipes)
            {
                if(recipe != null)
                    AssignRecipeToSlots(recipe);
            }
        }
        else
        {
            for(int i = 0; i < fileUtility.IngredList.Count; i++)
            {
                if (_ingredientHistory == fileUtility.IngredList[i])
                    _ingredientIndex = i;
            }

            //if we DO have an ingreidnet, only display recipes from that ingredient
            foreach (_devCrafting.Recipe recipe in fileUtility.SaveObject.recentRecipes[_ingredientIndex].recipes)
            {
                if (recipe != null)
                {
                    AssignRecipeToSlots(recipe);
                }
            }
        }

        StartCoroutine(FlickerLayoutGroup());
    }

    private void AdjustSize()
    {
        int width = 0;
        int number_of_children = GetComponentsInChildren<PotionHistorySlot>().Length;

        //420 = 400 of width and 20 of gap
        width = 420 * number_of_children;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, 0);
    }

    private void CreateHistorySlots()
    {
        int slotsToHave = 0;

        //if we do NOT have a Ingredient to Check, create slots for ALL Ingredients
        if (!_ingredientHistory)
        {
            foreach (SaveFile.RecipeList recipeArray in fileUtility.SaveObject.recentRecipes)
            {
                foreach (_devCrafting.Recipe recipe in recipeArray.recipes)
                {
                    if (recipe != null) { slotsToHave++; }
                }
            }
        }
        else
        {
            //if we DO have an ingredient to Check, only create slots for that Ingredient
            foreach (_devCrafting.Recipe recipe in fileUtility.SaveObject.recentRecipes[_ingredientIndex].recipes)
                if(recipe!= null) { slotsToHave++; }
        }

        while (_histories.Count < slotsToHave)
        {
            PotionHistorySlot newSlot = Instantiate(_slotPrefab, transform, true);
            _histories.Add(newSlot);
        }
    }

    private void AssignRecipeToSlots(_devCrafting.Recipe recipe)
    {
        //if recipe is a REAL recipe
        if (recipe != null)
        {
            //find a slot in the pool
            foreach (PotionHistorySlot slot in _histories)
            {
                //if that slot is NOT active (not re-enabled)
                if (!slot.isActiveAndEnabled)
                {
                    //set slot active and change its values to the REAL recipe
                    slot.gameObject.SetActive(true);
                    slot.SetRecipe(recipe);

                    //do not grab more than one slot
                    break;
                }
            }
        }
        else
        {
            //Debug.Log("found NULL recipe");
        }
    }

    IEnumerator FlickerLayoutGroup()
    {
        _layoutGroup.enabled = true;

        yield return new WaitForEndOfFrame();

        _layoutGroup.enabled = false;
    }
}
