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
    [SerializeField] private Text _nameText = null;

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
        AdjustText();
    }

    public void AdjustText()
    {
        //do NOT display non-active ingredient numbers, if 0 ingredient should be dynamically removed, if -1 it is infinite and should not display
        if (_ingredientData.Quantity > 0)
        {
            //if qtyText exists, we will display, but Text is optional so check for null first
            _qtyText?.text.Equals(_ingredientData.Quantity.ToString());
        }

        //if nameText exists, we will display, but it is optional
        _nameText?.text.Equals(_ingredientData.Name);

    }
}
