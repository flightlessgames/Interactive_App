using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementsUIController : MonoBehaviour
{
    [SerializeField] private GameObject _potionHistoryPanel = null;
    [SerializeField] private GameObject _unlocksPanel = null;

    private void OnEnable()
    {
        AchievementsController.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        AchievementsController.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(AchievementState state)
    {
        DisablePanels();

        switch (state)
        {
            case AchievementState.PotionHistory:
                _potionHistoryPanel.SetActive(true);
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
