using UnityEngine;
using System;
using UnityEngine.UI;

public class StateController: MonoBehaviour
{
    //starting at 0 is a fresh start, and an Invalid LoadInt, reseting gameplay every time unless the player selects Load->Slot#
    public static int LoadFilePosition { get; private set; } = 0;

    public int State { get; private set; } = 0;
    public static event Action<int> StateChanged = delegate { };

    [SerializeField] private Shop shop = null;
    //[SerializeField] private Text _mobileDebug = null;

    private void Awake()
    {
        if (fileUtility._shop == null)
        {
            fileUtility._shop = shop;
            //fileUtility._mobileDebug = _mobileDebug;
        }
    }

    private void Start()
    {
        ChangeState(1);
    }

    public void ChangeState(int stateIndex)
    {
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
