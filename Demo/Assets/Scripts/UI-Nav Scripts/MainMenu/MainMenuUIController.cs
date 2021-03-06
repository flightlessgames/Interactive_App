﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    //in theory we would want to identify the particular panel and avoid mis-matching by having a custom script on "rootPanel" and assign RootPanel _rootPanel, etc.
    [SerializeField] private GameObject _rootPanel = null;
    [SerializeField] private GameObject _loadDataSelectPanel = null;
    [SerializeField] private GameObject _loadDataConfirmPanel = null;
    [SerializeField] private Text _confirmText = null;
    [SerializeField] private GameObject _creditsPanel = null;

    private GameObject previousPanel = null;

    public int LoadFileSetting { get; private set; } = 0;

    private void OnEnable()
    {
        StateController.StateChanged += OnStateChanged;
    }


    private void OnDisable()
    {
        StateController.StateChanged -= OnStateChanged;
    }


    void OnStateChanged(int newState)
    {
        MenuState enumState = (MenuState)newState;

        switch (enumState)
        //most switch statements can be solved with proper inheritance or interface properties
        {
            case MenuState.Menu:
                //TODO RootUI Script
                StartCoroutine(ChangePanel(_rootPanel));
                //TODO Animations
                break;
            case MenuState.BeginPlay:
                //Begin Play Scene
                fileUtility.InitializeLoadSettings();
                SceneManager.LoadScene("CraftingTable");
                break;
            case MenuState.LoadSaveData:
                StartCoroutine(ChangePanel(_loadDataSelectPanel));
                break;
            case MenuState.LoadDataConfirm:
                StartCoroutine(AssignTextData());
                StartCoroutine(ChangePanel(_loadDataConfirmPanel));
                break;
            case MenuState.Credits:
                StartCoroutine(ChangePanel(_creditsPanel));
                break;
            default:
                Debug.Log("State not valid");
                //all panels will become disabled and none will become enabled. Look into player lock-out
                break;
        }
    }

    private IEnumerator AssignTextData()
    {
        if (fileUtility.SearchForSaveData(LoadFileSetting) != null)
        {
            int unlockCount = 0;
            foreach(int ingred in fileUtility._searchObject.ingredientsQuantity)
            {
                if (ingred > -1)
                    unlockCount++;
            }
            float AchievementPercent = unlockCount / fileUtility._searchObject.ingredientsQuantity.Length;
            Debug.Log("found search data");

            //needs to wait to update until reader has found SearchForSaveData, data. Leads to loaddata preview showing previous preview
            yield return new WaitForEndOfFrame();

            _confirmText.text =
                "Load File Number " + LoadFileSetting + 
           "\nFile Creation: " + fileUtility._searchObject.CreationTime +
           "\nLast Save: " + fileUtility._searchObject.RecentSaveTime +
           "\n" +
           "\nCurrent $: " + fileUtility._searchObject.gold +
           "\nAchievements: " + (int)(AchievementPercent*100) + "%";
            Debug.Log(fileUtility._searchObject.gold);
        }
    }

    private IEnumerator ChangePanel(GameObject changePanel)
    {
        changePanel.SetActive(true);
        yield return new WaitForSeconds(1);
        previousPanel?.SetActive(false);

        previousPanel = changePanel;
        if (previousPanel == _rootPanel)
            previousPanel = null;
    }

    public void ChangeLoadFile(int fileNum)
    {
        //temporary load setting, must be confirmed to move to StateController -> fileUtility
        LoadFileSetting = fileNum;
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
