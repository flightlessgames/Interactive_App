using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ShopFunctionController))]
public class ShopMenuUIController : MonoBehaviour
{
    [SerializeField] Text _goldText = null;

    private ShopFunctionController _shopFunction = null;

    private void Awake()
    {
        _shopFunction = GetComponent<ShopFunctionController>();
    }

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
        //only state change is leaving the scene, in all cases Save() after Shopping.
        fileUtility.Save();

        _goldText.text = fileUtility.SaveObject.gold.ToString();

        ShopState enumState = (ShopState)state;

        switch (enumState)
        {
            case ShopState.Shop:
                Debug.Log("Load Shop Deals");
                _shopFunction.RandomizeShop();
                break;
            case ShopState.Crafting:
                SceneManager.LoadScene("CraftingTable");
                break;
            case ShopState.Journal:
                SceneManager.LoadScene("Journal");
                break;
            default:
                Debug.Log("State Not Yet Implemented");
                break;
        }
    }


}
