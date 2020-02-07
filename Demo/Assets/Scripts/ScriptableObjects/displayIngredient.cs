using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class displayIngredient : MonoBehaviour
{
    [SerializeField] Ingredients_sObj _ingredient = null;
    public Ingredients_sObj Ingredient { get { return _ingredient; } }

    private Image _myImage = null;
    

    private void Awake()
    {
        _myImage = this.GetComponent<Image>();
    }

    private void Start()
    {
        _myImage.sprite = _ingredient.Image;
    }
}
