using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementsUIController : MonoBehaviour
{
    [SerializeField] private GameObject _potionHistoryPanel = null;
    [SerializeField] private PotionHistoryController _historyController = null;

    private void OnEnable()
    {
        StateController.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        StateController.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(int state)
    {
        AchievementState enumState = (AchievementState)state;

        switch (enumState)
        {
            case AchievementState.PotionHistory:
                _historyController.UpdateHistories();
                break;
            case AchievementState.Return:
                SceneManager.LoadScene("CraftingTable");
                break;
            default:
                Debug.Log("invalid state");
                break;
        }
    }
}