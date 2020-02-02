using System;
using UnityEngine;

public class CraftingController : MonoBehaviour
{
    public static CraftingState State { get; private set; } = CraftingState.None;
    public static event Action<CraftingState> StateChanged = delegate { };

    public void ChangeState(int newState)
    {
        State = (CraftingState)newState;
        StateChanged.Invoke(State);
    }

    void Start()
    {
        ChangeState(1);
    }
}
