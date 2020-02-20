using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalButtonPress : MonoBehaviour
{
    public PageManager pageManager;

    public void OnClick()
    {
        int index = transform.GetSiblingIndex();
        Debug.Log("INDEX IS " + index);
        pageManager.AddPage(index);
    }
}
