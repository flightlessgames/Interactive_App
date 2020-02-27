using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalButtonPress : MonoBehaviour
{
    public PageDisplay pageDisplay;

    public void OnClick()
    {
        int index = transform.GetSiblingIndex();
        Debug.Log("INDEX IS " + index);
        pageDisplay.setPage(index);
    }
}
