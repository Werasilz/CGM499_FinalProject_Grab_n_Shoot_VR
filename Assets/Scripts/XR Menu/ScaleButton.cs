using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleButton : MonoBehaviour
{
    private float floatScale = 1.3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Restart_Btn" || other.transform.name == "Exit_Btn")
        {
            other.transform.GetComponent<Animator>().SetBool("IsHover", true);
        }

        if (other.transform.name == "Yes" || other.transform.name == "No")
        {
            other.transform.GetComponent<Animator>().SetBool("IsHover", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Restart_Btn" || other.transform.name == "Exit_Btn")
        {
            other.transform.GetComponent<Animator>().SetBool("IsHover", false);
        }


        if (other.transform.name == "Yes" || other.transform.name == "No")
        {
            other.transform.GetComponent<Animator>().SetBool("IsHover", false);
        }
    }
}
