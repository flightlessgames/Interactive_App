using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldUIController : MonoBehaviour
{
    [SerializeField] private Text _text = null;
    private void OnEnable()
    {
        fileUtility.HasSaved += OnSave;
    }

    private void OnDisable()
    {
        fileUtility.HasSaved -= OnSave;
    }

    private void OnSave()
    {
        _text.text = fileUtility.SaveObject.gold.ToString();
    }
}
