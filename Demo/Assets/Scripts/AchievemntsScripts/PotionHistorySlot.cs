using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionHistorySlot : MonoBehaviour
{
    [SerializeField] private _devCrafting.Recipe _recipe = null;
    [SerializeField] private Text _title = null;
    [SerializeField] private Text _description = null;
    [SerializeField] private List<displayIngredient> _displays = new List<displayIngredient>();
    [SerializeField] private RawImage _potionColor = null;
    
    public _devCrafting.Recipe Recipe { get { return _recipe; } }

    public void SetRecipe(_devCrafting.Recipe newRecipe)
    {
        _recipe = newRecipe;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        _title.text = _recipe.recipeName;
        _potionColor.color = _recipe.color;

        if (_recipe.ingredientList == null)
            return;

        Vector3 score = Vector3.zero;

        for (int i=0; i<_recipe.ingredientList.Count; i++)
        {
            if (_recipe.ingredientList[i] == null)
                continue;

            _displays[i].SetIngredient(_recipe.ingredientList[i]);
            score += _recipe.ingredientList[i].Values;
        }

        _description.text = score.ToString();
    }
}
