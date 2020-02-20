using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class displayIngredient : MonoBehaviour
{
    [SerializeField] Ingredients_sObj _ingredientData = null;
    [SerializeField] Text _qtyText = null;
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
        AdjustQuanttiy();
    }

    public void SetIngredient(Ingredients_sObj ingredient)
    {
        _ingredientData = ingredient;
        UpdateDisplaySprite();
    }

    public void AdjustQuanttiy()
    {
        if(_qtyText != null)
        {
            if (_ingredientData.Quantity != -1)
                _qtyText.text = _ingredientData.Quantity.ToString();
            else
                _qtyText.text = "";
        }
    }
}
