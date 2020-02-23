using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class hotbarGroupController : MonoBehaviour
{
    private HorizontalLayoutGroup _horizontalGroup = null;
    private RectTransform _rectTransform = null;


    private float _scrollUnit = 0;
    private List<hotbarSlotController> _hotSlots = new List<hotbarSlotController>();

    private void Awake()
    {
        _horizontalGroup = GetComponent<HorizontalLayoutGroup>();
        _rectTransform = GetComponent<RectTransform>();

        StartCoroutine(FlickerLayoutGroup());
    }

    private void Start()
    {
        CountHotbar();
    }

    public void CountHotbar()
    {
        //initialized values for scrolling by counting how many children hotbarslots exist, and the size of those slots
        _hotSlots.Clear();

        foreach (hotbarSlotController slot in GetComponentsInChildren<hotbarSlotController>())
        {
            _hotSlots.Add(slot);
        }

        //adding together the first hotbar slot's Rect Width and Spacing from HorizontalLayoutGroup
        _scrollUnit += _hotSlots[0].GetComponent<RectTransform>().rect.width + _horizontalGroup.spacing;

        StartCoroutine(FlickerLayoutGroup());
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
        //using Mathf.Max lets us set 0 units to scroll if we have less than 6 hotbar slots. Otherwise the 7th will be offscreen and allow us to scroll 1 unit.
        if (_rectTransform.localPosition.x <= -_scrollUnit * Mathf.Max(_hotSlots.Count - 6, 0))
            return;

        _rectTransform.localPosition += Vector3.left * _scrollUnit;
    }
    #endregion

    IEnumerator FlickerLayoutGroup()
    {
        _horizontalGroup.enabled = true;

        //waits until end of frame to let the LayoutGroup make changes, before turning off
        yield return new WaitForEndOfFrame();

        _horizontalGroup.enabled = false;
        foreach (hotbarSlotController slot in _hotSlots)
        {
            slot.NewPosition();
        }
    }
}
