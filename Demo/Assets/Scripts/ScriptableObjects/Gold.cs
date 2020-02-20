using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gold", menuName = "Gold")]
public class Gold : ScriptableObject
{
    public int currentGold = 50;

    [SerializeField] private Sprite _image = null;
    public Sprite Image
    {
        get { return _image; }
    }

}