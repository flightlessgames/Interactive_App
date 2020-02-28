using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementsUIController : MonoBehaviour
{
    [SerializeField] private GameObject _potionHistoryPanel = null;
    [SerializeField] private GameObject _unlocksPanel = null;
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
        DisablePanels();
        AchievementState enumState = (AchievementState)state;

        switch (enumState)
        {
            case AchievementState.PotionHistory:
                _potionHistoryPanel.SetActive(true);
                _historyController.UpdateHistories();
                break;
            case AchievementState.Unlocks:
                _unlocksPanel.SetActive(true);
                break;
            case AchievementState.Return:
                SceneManager.LoadScene("CraftingTable");
                break;
            default:
                Debug.Log("invalid state");
                break;
        }
    }

    private void DisablePanels()
    {
        _potionHistoryPanel.SetActive(false);
        _unlocksPanel.SetActive(false);
    }
}
