using UnityEngine;
using System;

public class StateController: MonoBehaviour
{
    public int State { get; private set; } = 0;
    public static event Action<int> StateChanged = delegate { };
    
    public void ChangeState(int stateIndex)
    {
        State = stateIndex;
        StateChanged.Invoke(State);
    }

    private void Start()
    {
        ChangeState(1);
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
