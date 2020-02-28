using System;
using UnityEngine;

[Serializable]
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

    [TextArea]
    [SerializeField] private string _description = "...";
    public string Description { get { return _description; } }

    //ADDED: COST OF ITEM (lillianna)
    public int Cost
    {
        get
        {
            return (int)(_values.x + _values.y + _values.z);
        }
    }
    #endregion

    #region Dynamic/Gameplay Values
    [SerializeField] private int _quantity = -2; //-1 is inf placeholder
    public int Quantity { get { return _quantity; } }

    public void IncreaseQuantity(int value)
    {
        //if quantity is ""inf"" then ignore additions
        if (_quantity == -1)
            return;

        //if quantity is "locked" then unlock before adding
        if (_quantity == -2)
            _quantity = 0;

        _quantity += value;
    }

    public void DecreaseQuantity(int value)
    {
        //if our current quantity is ""inf"" then ignore subtractions
        if (_quantity == -1)
            return;

        //if we're somehow setting a non-""inf"" value to -2, override other procedure and allow "lock"
        if (_quantity - value == -2)
        {
            _quantity = -2;
            return;
        }

        //if our quantity would become less than 0, and not -2, an error has occured
        if(_quantity - value < 0)
        {
            Debug.Log("Decreasing by TOO MUCH");
            return;
        }

        _quantity -= value;
    }

    /// <summary>
    /// Only use in fileUtility.Load()
    /// </summary>
    /// <param name="value"></param>
    public void SetQuantity(int value)
    {
        _quantity = value;
    }

    #endregion
}
