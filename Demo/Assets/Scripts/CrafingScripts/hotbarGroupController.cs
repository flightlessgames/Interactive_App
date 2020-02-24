using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class hotbarGroupController : MonoBehaviour
{
    [Header("Required")]
    [SerializeField] private Shop _shopList = null;
    [SerializeField] private List<Ingredients_sObj> _defaultIngredients = new List<Ingredients_sObj>(3);
    [SerializeField] private hotbarSlotController _slotPrefab = null;

    private List<Ingredients_sObj> _allIngredients = new List<Ingredients_sObj>();

    private HorizontalLayoutGroup _horizontalGroup = null;
    private RectTransform _rectTransform = null;
    private float _scrollUnit = 0;

    private List<hotbarSlotController> _hotSlotsPool = new List<hotbarSlotController>();
    private int _currActiveSlots = 0;

    private void Awake()
    {
        _horizontalGroup = GetComponent<HorizontalLayoutGroup>();
        _rectTransform = GetComponent<RectTransform>();
        
        //initialize any pre-made slots in the scene as part of the pool of slots
        foreach(hotbarSlotController slot in _horizontalGroup.GetComponentsInChildren<hotbarSlotController>())
        {
            _hotSlotsPool.Add(slot);
        }
    }

    private void Start()
    {
        //set ALL INGREDIENTS to include Defaults as [0][1][2][3] and add ALL shoppableIngredients afterwards [4]...[61]
        _allIngredients.AddRange(_defaultIngredients);
        _allIngredients.AddRange(_shopList.shopInventory);

        CreateSlotPool();

        UpdateHotbar();
    }

    private void CreateSlotPool()
    {
        //if there are an inequal amount of slots compared to possible ingredients in the SlotsPool, make adjustments
        while (_allIngredients.Count != _hotSlotsPool.Count)
        {
            //more ingredients than slots
            if (_allIngredients.Count > _hotSlotsPool.Count)
            {
                //add a new slot, instantiated as a child of the horizontalgroup (a.k.a. hotbar)
                _hotSlotsPool.Add(Instantiate(_slotPrefab, _horizontalGroup.transform, true));
            }

            //more slots than ingredients
            if (_allIngredients.Count < _hotSlotsPool.Count)
            {
                //destroy a slot in the hotbar
                Destroy(_hotSlotsPool[0]);

                //hopefully the list will forget about the object by default?
                if (_hotSlotsPool[0] == null)
                {
                    Debug.Log("POOL NEVER FORGETS, by default");
                    _hotSlotsPool.RemoveAt(0);
                }
            }
        }
    }

    public void UpdateHotbar()
    {
        AssignActiveIngredients();

        CountActiveSlots();

        StartCoroutine(FlickerLayoutGroup());
    }

    private void AssignActiveIngredients()
    {
        //before setting any assignments, set all to INACTIVE (return to Pool)
        foreach(hotbarSlotController slot in _hotSlotsPool)
        {
            slot.gameObject.SetActive(false);
        }

        //going through the list of all ingredients
        foreach (Ingredients_sObj i in _allIngredients)
        {
            //if this particular ingredient has ANY quantity
            if (i.Quantity != 0)
            {
                //send it to the Hotbar
                foreach(hotbarSlotController slot in _hotSlotsPool)
                {
                    //skip active/used slots (not in Pool)
                    if (slot.isActiveAndEnabled)
                        continue;

                    //set an inactive slot to active (remove from Pool)
                    slot.gameObject.SetActive(true);

                    //set displayIngredient to display ingredient I
                    slot.Display.SetIngredient(i);

                    //do once and move on
                    break;
                }
            }
        }
    }

    private void CountActiveSlots()
    {
        _currActiveSlots = 0;

        foreach (hotbarSlotController slot in _hotSlotsPool)
        {
            //count each used slot from the pool
            if (slot.isActiveAndEnabled)
                _currActiveSlots++;

        }

        //adding together the first hotbar slot's Rect Width and Spacing from HorizontalLayoutGroup to assign a single "Unit" of "scroll," in case something changes at runtime
        _scrollUnit += _hotSlotsPool[0].GetComponent<RectTransform>().rect.width + _horizontalGroup.spacing;
    }

    /// <summary>
    /// Reset the Hotbar to Active Slots and reset their Defaulting positions for Drag function
    /// </summary>
    /// <returns></returns>
    IEnumerator FlickerLayoutGroup()
    {
        _horizontalGroup.enabled = true;

        //waits until end of frame to let the LayoutGroup make changes, before turning off
        yield return new WaitForEndOfFrame();

        _horizontalGroup.enabled = false;
        foreach (hotbarSlotController slot in _hotSlotsPool)
        {
            if(slot.isActiveAndEnabled)
                slot.NewPosition();
        }
    }

    #region ScrollBar for Overflow HotbarSlots
    public void ScrollLeft()
    {
        //localPosition.x initializes at 0, with the LEFTMOST items visible
        if (_rectTransform.localPosition.x >= 0)
            return;

        _rectTransform.localPosition += Vector3.right * _scrollUnit;
    }

    public void ScrollRight()
    {
        //using Mathf.Max lets us set 0 units to scroll if we have less than 6 hotbar slots. Otherwise the 7th will be offscreen and allow us to scroll 1 "unit"
        if (_rectTransform.localPosition.x <= -_scrollUnit * Mathf.Max(_hotSlotsPool.Count - 6, 0))
            return;

        _rectTransform.localPosition += Vector3.left * _scrollUnit;
    }
    #endregion
}
