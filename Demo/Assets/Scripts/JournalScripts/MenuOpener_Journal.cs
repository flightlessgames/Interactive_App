using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOpener_Journal : MonoBehaviour
{
    public GameObject Panel;
    public GameObject Button;

    public void OpenMenu()
    {
        if (Panel != null)
        {
            Animator animator = Panel.GetComponent<Animator>();
            if (animator != null)
            {
                bool isOpen = animator.GetBool("open");
                animator.SetBool("open", !isOpen);
                Debug.Log("ITS OPENING");
            }
        }
    }

    public void MoveButton()
    {
        if (Button != null)
        {
            Animator animator = Button.GetComponent<Animator>();
            if (animator != null)
            {
                bool isOpen = animator.GetBool("moved");
                animator.SetBool("moved", !isOpen);
                Debug.Log("ITS OPENING");

                if (isOpen == true)
                {
                    Button.GetComponentInChildren<Text>().text = "<";
                }
                else
                {
                    Button.GetComponentInChildren<Text>().text = ">";
                }
            }
        }
    }
}

