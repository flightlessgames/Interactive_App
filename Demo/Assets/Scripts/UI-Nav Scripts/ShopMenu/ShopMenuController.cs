using System;
using UnityEngine;

public class ShopMenuController : MonoBehaviour
{
    public static event Action<ShopState> StateChanged = delegate { };
    public static ShopState State { get; private set; } = ShopState.None;

    public void ChangeState(int newState)
    {
        State = (ShopState)newState;
        StateChanged.Invoke(State);
    }

    private void Start()
    {
        ChangeState(1);
    }
}
