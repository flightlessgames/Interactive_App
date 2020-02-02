using UnityEngine;
using UnityEngine.SceneManagement;

public class CraftingUIController : MonoBehaviour
{

    private void OnEnable()
    {
        CraftingController.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        CraftingController.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(CraftingState state)
    {

        switch (state)
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
