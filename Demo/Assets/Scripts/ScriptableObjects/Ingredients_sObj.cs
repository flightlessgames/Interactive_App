using UnityEngine;

[CreateAssetMenu(fileName ="New_Ingredient_sObj", menuName = "sObj/Ingredient")]
public class Ingredients_sObj : ScriptableObject
{
    #region Static/Designer Values
    [SerializeField] private Vector3 _values = Vector3.zero;
    public Vector3 Values { get { return _values; } }

    [SerializeField] private string _name = "...";
    public string Name { get { return _name; } }

    [SerializeField] private Sprite _image = null;
    public Sprite Image { get { return _image; } }

    #endregion

    #region Dynamic/Gameplay Values
    [SerializeField] private int _quantity = -1; //-1 is inf placeholder
    public int Quantity { get { return _quantity; } }

    public void IncreaseQuantity(int value)
    {
        if (_quantity < 0) { _quantity = 0; }
        _quantity += value;
    }

    public void DecreaseQuantity(int value) //currently no check on Quantity, 0 functions like -1 in _devCrafting
    {
        if (_quantity < 0)   //using -1 as inf value
        {
            Debug.Log("Used a[n] " + Name);
            return;
        }

        if(_quantity - value < 0)
        {
            Debug.Log("using too many " + Name);
            return;
        }

        _quantity -= value;
        Debug.Log(Name + " quantity is now " + _quantity);
    }

    #endregion
}
