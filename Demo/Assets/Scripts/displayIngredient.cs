using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class displayIngredient : MonoBehaviour
{
    [Header("Required Initialized Setting")]
    [Tooltip("Can be _nullObject")]
    [SerializeField] private Ingredients_sObj _ingredientData = null;
    public Ingredients_sObj IngredientData { get { return _ingredientData; } }

    [Header("Optional")]
    [Tooltip("For Hotbar Use Primarily")]
    [SerializeField] private Text _qtyText = null;

    private Image _myImage = null;    

    private void Awake()
    {
        _myImage = GetComponent<Image>();
    }

    private void Start()
    {
        UpdateDisplaySprite();
    }

    public void SetIngredient(Ingredients_sObj ingredient)
    {
        _ingredientData = ingredient;
        UpdateDisplaySprite();
    }

    private void UpdateDisplaySprite()
    {
        _myImage.sprite = _ingredientData.Image;
        AdjustQuanttiy();
    }

    public void AdjustQuanttiy()
    {
        if(_qtyText != null)
        { 
            if (_ingredientData.Quantity < 1) //do NOT display non-active ingredient numbers, if 0 ingredient should be dynamically removed, if -1 it is infinite and should not display
                _qtyText.text = _ingredientData.Quantity.ToString();
            else
                _qtyText.text = "";
        }
    }
}
