using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionHistorySlot : MonoBehaviour
{
    [SerializeField] private _devCrafting.Recipe _recipe = null;
    [SerializeField] private Text _title = null;
    [SerializeField] private Text _description = null;
    [SerializeField] private List<displayIngredient> _ingredientImages = new List<displayIngredient>();
    [SerializeField] private Image _potionColor = null;
    
    public _devCrafting.Recipe Recipe { get { return _recipe; } }

    public void SetRecipe(_devCrafting.Recipe newRecipe)
    {
        _recipe = newRecipe;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        Debug.Log("updating recipe history");
        _title.text = _recipe.recipeName;
        _potionColor.color = _recipe.color;
        for(int i = 0; i < _recipe.input.Count; i++)
        {
            _ingredientImages[i].SetIngredient(_recipe.input[i]);
        }
    }
}
