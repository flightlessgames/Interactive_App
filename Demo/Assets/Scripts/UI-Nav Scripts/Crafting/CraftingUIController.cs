using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CraftingUIController : MonoBehaviour
{
    public Ingredients_sObj CurrIngredient { get; private set; } = null;

    [SerializeField] GameObject _potionResultsUI = null;
    [SerializeField] _devCrafting _crafing = null;
    [SerializeField] private Gold _gold = null;
    [SerializeField] Text _goldText = null;

    private Vector3 _clickDown = Vector3.zero;

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
        _goldText.text = _gold.currentGold.ToString();
        CraftingState enumState = (CraftingState)state;

        switch (enumState)
        {
            case CraftingState.Crafting:
                _potionResultsUI.SetActive(false);
                _crafing.Clear();
                break;

            case CraftingState.Achievements:
                SceneManager.LoadScene("Achievement");
                break;

            case CraftingState.Store:
                SceneManager.LoadScene("Shop");
                break;

            case CraftingState.Journal:
                SceneManager.LoadScene("Journal");
                break;

            case CraftingState.PotionResult:
                _potionResultsUI.SetActive(true);
                _gold.currentGold += 10;

                break;

            default:
                Debug.Log("state not implemented");
                break;
        }
    }

    public void HoldIngredient(Ingredients_sObj ingredient)
    {
        //called from the HotBar script, tells the controller to remember the data passed from the HotBar's _ingred
        Debug.Log("Set Held ingredient to: " + ingredient);
        CurrIngredient = ingredient;
    }
}
