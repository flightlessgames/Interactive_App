using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Spinner : MonoBehaviour
{
    [SerializeField] private float _spinmod = 1;
    private RectTransform _transform = null;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        _transform.Rotate(Vector3.forward * _spinmod);
    }
}
