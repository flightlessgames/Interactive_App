using UnityEngine;
using UnityEngine.SceneManagement;

public class CraftingUIController : MonoBehaviour
{

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
        CraftingState enumState = (CraftingState)state;

        switch (enumState)
        {
            case CraftingState.Achievements:
                SceneManager.LoadScene("Achievement");
                break;
            case CraftingState.Store:
                SceneManager.LoadScene("Shop");
                break;
            case CraftingState.Journal:
                SceneManager.LoadScene("Journal");
                break;
            default:
                Debug.Log("state not implemented");
                break;
        }
    }
}
