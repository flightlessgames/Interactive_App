using UnityEngine;
using System;

public class AchievementsController : MonoBehaviour
{
    public AchievementState State { get; private set; } = AchievementState.None;
    public static event Action<AchievementState> StateChanged = delegate { };
    
    public void ChangeState(int stateIndex)
    {
        State = (AchievementState)stateIndex;
        StateChanged.Invoke(State);
    }

    private void Start()
    {
        ChangeState(1);
    }
}
