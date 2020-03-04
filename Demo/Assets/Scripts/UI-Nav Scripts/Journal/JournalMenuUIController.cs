using UnityEngine;
using UnityEngine.SceneManagement;

public class JournalMenuUIController : MonoBehaviour
{

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
        _historyController.UpdateHistories();

        JournalState enumState = (JournalState)state;
        switch (enumState)
        {
            case JournalState.Journal:
                Debug.Log("Load Journal Pg 1");
                break;
            case JournalState.Crafting:
                SceneManager.LoadScene("CraftingTable");
                break;
            case JournalState.Shop:
                SceneManager.LoadScene("Shop");
                break;
            default:
                Debug.Log("State not yet Implimented");
                break;
        }
    }

}
