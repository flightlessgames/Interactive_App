using UnityEngine;
using UnityEngine.SceneManagement;

public class JournalMenuUIController : MonoBehaviour
{
    private void OnEnable()
    {
        JournalMenuController.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        JournalMenuController.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(JournalState state)
    {
        switch (state)
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
