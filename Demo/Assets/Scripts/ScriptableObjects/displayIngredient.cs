using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class displayIngredient : MonoBehaviour
{
    [SerializeField] Ingredients_sObj _ingredientData = null;
    public Ingredients_sObj IngredientData { get { return _ingredientData; } }

    private Image _myImage = null;    

    private void Awake()
    {
        _myImage = GetComponent<Image>();
    }

    private void Start()
    {
        UpdateDisplaySprite();
    }

    private void UpdateDisplaySprite()
    {
        _myImage.sprite = _ingredientData.Image;
    }

    public void SetIngredient(Ingredients_sObj ingredient)
    {
        _ingredientData = ingredient;
        UpdateDisplaySprite();
    }
}
