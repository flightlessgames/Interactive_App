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
    [SerializeField] private int _quantity = -1; //-1 is inf placeholder
    public int Quantity { get { return _quantity; } }

    public void IncreaseQuantity(int value)
    {
        if (_quantity < 0) return; 

        _quantity += value;
    }

    public void DecreaseQuantity(int value)
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
