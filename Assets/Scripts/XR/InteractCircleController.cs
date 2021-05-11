using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractCircleController : MonoBehaviour
{
    private Image interactCircle1;
    private Image interactCircle2;

    void Awake()
    {
        interactCircle1 = transform.Find("Interact Circle1").GetComponentInChildren<Image>();
        interactCircle2 = transform.Find("Interact Circle2").GetComponentInChildren<Image>();
    }

    public void ShowInteractCircle(bool value)                                                         
    {
        interactCircle1.gameObject.SetActive(value);
        interactCircle2.gameObject.SetActive(value);
    }

    public void SetAnimationInteract(bool value)                                                      
    {
        interactCircle1.GetComponent<Animator>().SetBool("isHover", value);
        interactCircle2.GetComponent<Animator>().SetBool("isHover", value);
    }
}
