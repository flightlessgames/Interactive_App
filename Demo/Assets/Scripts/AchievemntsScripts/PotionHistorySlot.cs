using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionHistorySlot : MonoBehaviour
{
    [SerializeField] private _devCrafting.Recipe _recipe = null;
    [SerializeField] private Text _title = null;
    [SerializeField] private Text _description = null;
    [SerializeField] private List<Image> _ingredientImages = new List<Image>();
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

        for(int i = 0; i < _recipe.ingredientList.Count; i++)
        {
            //if a recipe has a null ingredient (should be limited to List.Count size, debugging
            if (_recipe.ingredientList[i] == null)
            {
                Debug.Log("ingredient is null");
                continue;
            }

            _ingredientImages[i].sprite = _recipe.ingredientList[i];
            
        }
    }
}
