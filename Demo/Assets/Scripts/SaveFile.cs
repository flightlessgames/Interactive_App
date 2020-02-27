using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
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
    /// array of all 58 unlockable ingredients
    /// </summary>
    public int[] ingredientsQuantity;

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
        Debug.Log("Creating a NEW SaveFile");

        CreationTime = Time.time;

        #region Recipe[]
        //new empty list of recipes
        recentRecipes = new _devCrafting.Recipe[5];

        _devCrafting.Recipe recipe = new _devCrafting.Recipe(
            "Baby's First Recipe",
            Color.white,
            new List<Ingredients_sObj>());

        AddRecipe(recipe);
        #endregion

        //double utility out of the ingredientsQty:
        /*
         * if == -2,    locked, 
         * if == -1,    infinite
         * if  < -1,    unlocked and read qty.
         */
        ingredientsQuantity = new int[58];
        foreach(int i in ingredientsQuantity)
        {
            i.Equals(-2);
            //using .Equals because of foreach properties
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
        ingredientsQuantity = previousSave.ingredientsQuantity;
        gold = previousSave.gold;
    }

    /// <summary>
    /// Adds Recipe data as 1st slot, pushing all slots forwards and Popping 5th
    /// </summary>
    public void AddRecipe(_devCrafting.Recipe newRecipe)
    {
        //counting backwards for posterity
        for (int i = recentRecipes.Length - 1; i >= 0; i--)
        {
            //move each slot to overwrite next slot in list
            if (i != recentRecipes.Length-1)
            {
                //eg, starting with [3], [4] = [3], then [2], [3] = [2], so on...
                recentRecipes[i + 1] = recentRecipes[i];
            }

            //ignore last (i == [4]) as it is overwritten by [3]

            //set first recipe to new recipe
            if (i == 0)
            {
                recentRecipes[0] = newRecipe;
            }
        }
    }
}