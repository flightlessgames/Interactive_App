using UnityEngine;
using System;

public class MainMenuController : MonoBehaviour
{
    public MenuState State { get; private set; } = MenuState.None;
    public static event Action<MenuState> StateChanged = delegate { };
    public static event Action<string> MissionChanged = delegate { };

    public string ActiveMission { get; private set; } = "..."; //see ... as desginer input

    public void ChangeState ( int stateIndex)
    {
        State = (MenuState)stateIndex; //takes int-stateIndex and converts it into type 'MenuState'     //akin to (int)100.0f
        StateChanged.Invoke(State);
    }

    public void ChangeMission(string missionName)//for simplicity, missions are strings, look into ScriptableObjects
    {
        ActiveMission = missionName;
        MissionChanged.Invoke(ActiveMission);
    }

    private void Start()
    {
        ChangeState(1); //activate root state (1)
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
