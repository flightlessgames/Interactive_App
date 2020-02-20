using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    public GameObject[] pages;


    public void AddPage(int index)
    {
        string panelName = pages[index].name;
        Debug.Log(panelName);

        if(GameObject.Find(panelName) == true)
        {
            Debug.Log("Exists");
            GameObject page = GameObject.Find(panelName);
            page.transform.SetAsLastSibling();
        }
    }
}
