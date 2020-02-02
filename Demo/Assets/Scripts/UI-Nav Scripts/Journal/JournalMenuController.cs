using System;
using UnityEngine;

public class JournalMenuController : MonoBehaviour
{
    public static JournalState State = JournalState.None;
    public static event Action<JournalState> StateChanged = delegate { };

    public void ChangeState(int newState)
    {
        State = (JournalState)newState;
        StateChanged.Invoke(State);
    }

    private void Start()
    {
        ChangeState(1);
    }
}
