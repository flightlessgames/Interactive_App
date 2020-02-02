using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    //in theory we would want to identify the particular panel and avoid mis-matching by having a custom script on "rootPanel" and assign RootPanel _rootPanel, etc.
    [SerializeField] GameObject _rootPanel = null;
    [SerializeField] GameObject _loadDataSelectPanel = null;
    [SerializeField] GameObject _loadDataConfirmPanel = null;
    [SerializeField] GameObject _creditsPanel = null;

    [SerializeField] Text _missionTextUI = null;


    private void OnEnable()
    {
        MainMenuController.StateChanged += OnStateChanged;
        MainMenuController.MissionChanged += OnMissionChanged;
    }


    private void OnDisable()
    {
        MainMenuController.StateChanged -= OnStateChanged;
        MainMenuController.MissionChanged -= OnMissionChanged;
    }


    void OnStateChanged(MenuState newState)
    {
        //disable all, then turn on proper case.
        DisablePanels();

        switch (newState)
        //most switch statements can be solved with proper inheritance or interface properties
        {
            case MenuState.Root:
                //TODO RootUI Script
                _rootPanel.gameObject.SetActive(true);
                //TODO Animations
                break;
            case MenuState.BeginPlay:
                //Begin Play Scene
                SceneManager.LoadScene("CraftingTable");
                break;
            case MenuState.LoadSaveData:
                _loadDataSelectPanel.gameObject.SetActive(true);
                break;
            case MenuState.LoadDataConfirm:
                _loadDataConfirmPanel.gameObject.SetActive(true);
                break;
            case MenuState.Credits:
                _creditsPanel.gameObject.SetActive(true);
                break;
            default:
                Debug.Log("State not valid");
                //all panels will become disabled and none will become enabled. Look into player lock-out
                break;
        }
    }


    private void OnMissionChanged(string newMission)
    {
        _missionTextUI.text = newMission;
    }

    private void DisablePanels()
    {
        //TODO check if panel is currently active, if so Animate
        _rootPanel.gameObject.SetActive(false);
        _loadDataSelectPanel.gameObject.SetActive(false);
        _loadDataConfirmPanel.gameObject.SetActive(false);
        _creditsPanel.gameObject.SetActive(false);
    }
}
