using System;
using System.Collections.Generic;
using System.Linq;
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
    public RecipeList[] recentRecipes;

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
        CreationTime = Time.time;

        #region Recipe[]
        //new empty list of recipes
        recentRecipes = new RecipeList[61];
        for (int i = 0; i < recentRecipes.Length; i++)
        {
            if (recentRecipes[i] == null)
                recentRecipes[i] = new RecipeList();
        }

        _devCrafting.Recipe recipe = new _devCrafting.Recipe(
            "Baby's First Recipe",
            Color.white,
            new List<Ingredients_sObj>()
            );

        InputNewRecipe(recipe);
        #endregion

        //double utility out of the ingredientsQty:
        /*
         * if == -2,    locked, 
         * if == -1,    infinite
         * if  < -1,    unlocked and read qty.
         */
        ingredientsQuantity = Enumerable.Repeat(-2, 58).ToArray();

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
    /// Find the recipe[] assigned to each Ingredient in recipe.ingredientList, then assign recipe to that array
    /// </summary>
    public void InputNewRecipe(_devCrafting.Recipe newRecipe)
    {
        //identify each ingedient and check if it's UNIQUE in the recipe
        
        //by cycling through j=i, we don't ever compare backwards, so if [0] == [1], we skip 0, but then we never check if [1] == [0] and use 1, once, for uniqueness
        for (int i=0; i < newRecipe.ingredientList.Count; i++)
        {
            bool unique = true;

            //starting with ingredient i, and every ingredient AFTER i
            for(int j=i; j < newRecipe.ingredientList.Count; j++)
            {
                //if i == j, we're comparing ingredient to itself, skip
                //(guarantee on initialized j=i, but unsafe to declare j = i + 1, due to arrayLimit / bounds)
                if (i == j) continue;

                //if ingredient i is equals to ingredient j, then the ingredient is known to be Not-Unique
                if(newRecipe.ingredientList[i] == newRecipe.ingredientList[j])
                {
                    unique = false;
                    break;
                }
            }

            //if we never found an ingredient[i] == ingredient[j], this ingredient never repeated in our recipe
            if (unique)
            {
                //find/assign the ingredient's recentRecipes[] in the recentRecipes List<>, and AddRecipeToArray

                for (int j = 0; j < fileUtility.IngredList.Count; j++)
                {
                    //compare to FileUtility.IngredientList, the definitive list of all ingredients, to determine position in recentRecipes List<>
                    if (newRecipe.ingredientList[i] == fileUtility.IngredList[j])
                        recentRecipes[j].AddRecipeToArray(newRecipe);
                }
            }
        }
    }

    [Serializable]
    public class RecipeList
    {
        public _devCrafting.Recipe[] recipes = new _devCrafting.Recipe[5];

        /// <summary>
        /// Assigns newRecipe to first slot in recipe[], moves all other recipes down (deleting 5th)
        /// </summary>
        public void AddRecipeToArray(_devCrafting.Recipe newRecipe)
        {
            //counting backwards for convenience
            for (int i = recipes.Length - 1; i >= 0; i--)
            {
                //move each slot to overwrite next slot in list
                if (i != recipes.Length - 1)
                {
                    //eg, starting with [3], [4] = [3], then [2], [3] = [2], so on...
                    recipes[i + 1] = recipes[i];
                }

                //ignore last (i == [4]) as it is overwritten by [3]

                //set first recipe to new recipe
                if (i == 0)
                {
                    recipes[0] = newRecipe;
                }
            }
        }
    }
}