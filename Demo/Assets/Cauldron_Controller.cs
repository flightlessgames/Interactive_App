using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Cauldron_Controller : MonoBehaviour
{
    [Header("Required")]
    [SerializeField] private _devCrafting _crafting = null;

    [Header("Settings")]
    [SerializeField] private float _craftDelay = 0.5f;

    public void OnClick()
    {
        StartCoroutine(Craft());
    }

    IEnumerator Craft()
    {
        yield return new WaitForSeconds(_craftDelay);

        _crafting.CraftPotion();
    }
}
