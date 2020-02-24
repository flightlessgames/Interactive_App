using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//we're requiring type of Button, but not using in the script. Use Designer/Component interface to link the Button's OnClick to this script's OnClick();
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(displayIngredient))]
public class hotbarSlotController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] CraftingUIController _craftingController = null;
    
    //using displayIngredient script to Visualise the Ingredient_sObj data
    private displayIngredient _display = null;
    public displayIngredient Display { get { return _display; } }

    private Vector3 _startPosition = Vector3.zero;

    List<RaycastResult> _hitObjects = new List<RaycastResult>();
    private const string TARGET_TAG = "craftingSlot";

    private void Awake()
    {
        _display = GetComponent<displayIngredient>();
    }

    private void Start()
    {
        NewPosition();
    }

    //using a generic OnClick() function to link the Button component's commands to this script. Useful for Computer & Touch Devices.
    public void OnClick()
    {
        Debug.Log("OnClick()");
        _craftingController.HoldIngredient(_display.IngredientData);
    }

    public void NewPosition()
    {
        //using local position without HotBarGroup parent, as parent "scrolls" our local position will remain the same
        _startPosition = transform.localPosition;
    }

    #region Drag + Drop
    //inspirition to use the IDragHandler from Jayanam's tutorials on YouTube: https://www.youtube.com/watch?v=Pc8K_DVPgVM
    //from their Unity Invetory Tutorial Series, UI Drag & Drop

    public void OnDrag(PointerEventData eventData)  //interfaces *must*? be public?
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
            transform.position = Input.mousePosition;

        if (SystemInfo.deviceType == DeviceType.Handheld)
            transform.position = Input.touches[0].position;

        //when we start draging, if there's no ingredient we add ours
        if (_craftingController.CurrIngredient != _display.IngredientData)
        {
            Debug.Log("OnDrag() {CurrIngredient}");
            _craftingController.HoldIngredient(_display.IngredientData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = _startPosition;
        //looking to put code here to mess with the craftingSlots to enable easy Drag+Drop

        craftingSlotController slot = GetCraftingSlotUnderMouse();
        slot?.OnClick();
    }

    //additional code adapted from Omnirift on YouTube: https://www.youtube.com/watch?v=fhBJWTO09Lw
    private GameObject GetObjectUnderMouse()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        
        pointer.position = Input.mousePosition;

        EventSystem.current.RaycastAll(pointer, _hitObjects);

        if (_hitObjects.Count <= 0) return null;

        return _hitObjects[0].gameObject;
    }

    private craftingSlotController GetCraftingSlotUnderMouse()
    {
        GameObject clickedObject = GetObjectUnderMouse();

        //will store value of craftingSlotController if clickedObject is not null and has a component
        craftingSlotController slot = clickedObject?.GetComponent<craftingSlotController>();
        
        //will return null if slot is invalid
        return slot;
    }
    #endregion
}
