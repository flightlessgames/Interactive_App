using System.Collections.Generic;
using UnityEngine;

[serializable]
public class SaveFile
{
    //all values must be public for JsomUtility to work... not happy with exposed values...
    public float CreationTime = 0;
    public float RecentSaveTime = 0;

    /// <summary>
    /// list of previous specific potions crafted, up to a limit for Achievement-History Page
    /// </summary>
    public _devCrafting.Recipe[] recentRecipes;

    /// <summary>
    /// List of previous "Unlocked" potion recipes "known" for Achievements Page
    /// </summary>
    public bool[] unlockedIngredients;

    //TODO get ALL ingrasdasdedient_sObj.Quantity into a List<> somehow?

    //all edited ingredient_sObj / journal_sObj datapoints (vector3)?
    //TODO get ALL journal_sObj? or re-use ingredient_sObj list to calculate this value

    /// <summary>
    /// Current Gold Amount
    /// </summary>
    public int gold;

    //time spent playing?


    //default constructor, default starting game values
    public SaveFile()
    {
        CreationTime = Time.time;

        //new empty list of recipes
        recentRecipes = new _devCrafting.Recipe[5];

        //default empty recipe
        Debug.Log("No recipe");
        _devCrafting.Recipe recipe = new _devCrafting.Recipe("Baby's First Recipe", Color.white, new List<Ingredients_sObj>());
        AddRecipe(recipe);

        //new array of all ingredient/bool set to false
        unlockedIngredients = new bool[60]; //60 is stand-in of Ingredients_sObj size, will change to reflect Potion List size
        for(int i=0; i<unlockedIngredients.Length; i++)
        {
            //first 4 are Cancel, Freebie1, Freebie2, Freebie3, are always true? or we skip those for verifying states...
            if (i < 4)
            {
                unlockedIngredients[i].Equals(true);
            }
            else
            {
                unlockedIngredients[i].Equals(false);
            }
        }

        //start with 100g
        gold = 100;
    }

    //copy constructor to work with JsomUtility?
    public SaveFile(SaveFile previousSave)
    {
        CreationTime = previousSave.CreationTime;
        RecentSaveTime = previousSave.RecentSaveTime;
        recentRecipes = previousSave.recentRecipes;
        unlockedIngredients = previousSave.unlockedIngredients;
        gold = previousSave.gold;
    }

    /// <summary>
    /// Adds Recipe data as 1st slot, pushing all slots forwards and Popping 5th
    /// </summary>
    public void AddRecipe(_devCrafting.Recipe newRecipe)
    {
        //adds a recipe to the array
        Debug.Log("Adding a Recipe");

        for (int i = recentRecipes.Length - 1; i >= 0; i--)
        {
            //move each slot to overwrite next slot in list
            if (i != 4)
            {
                recentRecipes[i + 1] = recentRecipes[i];
            }

            //ignore last (i == [4]) as it is overwritten

            //set first recipe to new recipe
            if (i == 0)
            {
                recentRecipes[0] = newRecipe;
                Debug.Log(recentRecipes[0].recipeName + " recipe");
            }
        }
    }
}