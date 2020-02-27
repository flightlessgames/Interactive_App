using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class PotionHistoryController : MonoBehaviour
{
    private VerticalLayoutGroup _verticalGroup = null;

    [SerializeField] private PotionHistorySlot _slotPrefab = null;
    [SerializeField] private List<PotionHistorySlot> _histories = new List<PotionHistorySlot>();
    

    private void Awake()
    {
        _verticalGroup = GetComponent<VerticalLayoutGroup>();
        StartCoroutine(FlickerLayoutGroup());
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
                Debug.Log("found recipe");
                //find a slot in the pool
                foreach(PotionHistorySlot slot in _histories)
                {
                    //if that slot is unused (not re-enabled)
                    if (slot.isActiveAndEnabled)
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
        _verticalGroup.enabled = true;

        yield return new WaitForEndOfFrame();

        _verticalGroup.enabled = false;
    }
}
