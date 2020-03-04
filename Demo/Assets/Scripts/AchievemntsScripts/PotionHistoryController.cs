using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalOrVerticalLayoutGroup))]
public class PotionHistoryController : MonoBehaviour
{
    private HorizontalOrVerticalLayoutGroup _layoutGroup = null;

    [SerializeField] private PotionHistorySlot _slotPrefab = null;
    [SerializeField] private List<PotionHistorySlot> _histories = new List<PotionHistorySlot>();

    [SerializeField] private bool _needsAdjustment = false;

    private void Awake()
    {
        _layoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
        StartCoroutine(FlickerLayoutGroup());
        CreateHistorySlots();
        AdjustSize();
    }

    private void AdjustSize()
    {
        if (_needsAdjustment)
        {
            int width = 0;
            int number_of_children = GetComponentsInChildren<PotionHistorySlot>().Length;

            //420 = 400 of width and 20 of gap
            width = 420 * number_of_children;
            GetComponent<RectTransform>().sizeDelta = new Vector2(width , 0);
            Debug.Log("width " + width + ", number of children " + number_of_children);
        }
    }

    private void CreateHistorySlots()
    {
        int slotsToHave = 0;
        foreach(_devCrafting.Recipe recipe in fileUtility.SaveObject.recentRecipes)
        {
            if(recipe != null) { slotsToHave++; }
        }

        while (_histories.Count < slotsToHave)
        {
            PotionHistorySlot newSlot = Instantiate(_slotPrefab, transform, true);
            _histories.Add(newSlot);
        }
    }

    public void UpdateHistories()
    {
        //disable all slots to return to pool
        foreach(PotionHistorySlot slot in _histories)
        {
            slot.gameObject.SetActive(false);
        }

        //for each recipe in our save file (all recent recipes)
        foreach(_devCrafting.Recipe recipe in fileUtility.SaveObject.recentRecipes)
        {
            //if recipe is a REAL recipe
            if(recipe != null)
            {
                //find a slot in the pool
                foreach(PotionHistorySlot slot in _histories)
                {
                    //if that slot is NOT active (not re-enabled)
                    if (!slot.isActiveAndEnabled)
                    {
                        //set slot active and change its values to the REAL recipe
                        slot.gameObject.SetActive(true);
                        slot.SetRecipe(recipe);

                        //do not grab more than one slot
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("found NULL recipe");
            }
        }
    }

    IEnumerator FlickerLayoutGroup()
    {
        _layoutGroup.enabled = true;

        yield return new WaitForEndOfFrame();

        _layoutGroup.enabled = false;
    }
}
