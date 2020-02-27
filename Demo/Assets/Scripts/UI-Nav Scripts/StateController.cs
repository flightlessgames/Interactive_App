using UnityEngine;
using System;

public class StateController: MonoBehaviour
{
    //starting at 0 is a fresh start, and an Invalid LoadInt, reseting gameplay every time unless the player selects Load->Slot#
    public static int LoadFilePosition { get; private set; } = 0;

    public int State { get; private set; } = 0;
    public static event Action<int> StateChanged = delegate { };

    [SerializeField] private Shop shop = null;

    private void Awake()
    {
        if(fileUtility._shop == null)
            fileUtility._shop = shop;
    }

    private void Start()
    {
        ChangeState(1);
    }

    public void ChangeState(int stateIndex)
    {
        //cause a save every time we try to change the state, might be too many??
        //ensures data is persistant between all scenes, and saves often so data is probably not lost on exit
        fileUtility.Save();

        State = stateIndex;
        StateChanged.Invoke(State);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void ChangeLoadFile(int load)
    {
        //permanent load pointer change
        LoadFilePosition = load;
        Debug.Log("Now load file " + LoadFilePosition);

        //initialize -> then run a new Load();
        fileUtility.InitializeLoadSettings();
    }
}
