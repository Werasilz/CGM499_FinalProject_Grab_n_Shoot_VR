using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    XRRayInteractor XRRayInteractor;
    Animator animator;

    private float speed = 10;

    // Check have item in hands
    [HideInInspector] public bool isHandleItem;

    #region Value of Controller                      
    private float gripTarget;
    private float gripCurrent;
    private float triggerTarget;
    private float triggerCurrent;
    #endregion

    #region Name of bool in animator
    private string animatorGripParam = "Grip";
    private string animatorTriggerParam = "Trigger";
    private string animatorFistParam = "Fist";
    #endregion

    void Awake()
    {
        XRRayInteractor = GetComponentInParent<XRRayInteractor>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        XRRayInteractor.selectEntered.AddListener(OnSelectEnter);
        XRRayInteractor.selectExited.AddListener(OnSelectExit);
    }

    void Update()
    {
        AnimateHand();
    }

    void OnSelectEnter(SelectEnterEventArgs args)
    {
        // Set bool when grab something ///// bug select teleport anchor /////
        if (XRRayInteractor.isSelectActive)
        {
            isHandleItem = true;
        }

        // Warp Point System
        if (XRRayInteractor.selectTarget.gameObject.tag == "WarpPoint")
        {
            PlayerManager.Instance.currentArea = XRRayInteractor.selectTarget.gameObject.transform.GetChild(0).GetChild(0).GetComponent<CheckPlayArea>().areaNumber;

            MeshRenderer meshRenderer = XRRayInteractor.selectTarget.GetComponent<MeshRenderer>();
            meshRenderer.enabled = false;

            SphereCollider sphereCollider = XRRayInteractor.selectTarget.GetComponent<SphereCollider>();
            sphereCollider.enabled = false;
        }

        GetComponentInParent<XRInteractorLineVisual>().enabled = false;
    }

    void OnSelectExit(SelectExitEventArgs args)
    {
        isHandleItem = false;
        GetComponentInParent<XRInteractorLineVisual>().enabled = true;
        ResetValue();
    }

    public void ResetValue()
    {
        gripCurrent = 0;
        gripTarget = 0;
        triggerCurrent = 0;
        triggerTarget = 0;
        animator.SetFloat(animatorGripParam, gripCurrent);
        animator.SetFloat(animatorTriggerParam, triggerCurrent);
        animator.SetFloat(animatorFistParam, gripCurrent);
    }

    #region Get Value from Controller
    public void SetGrip(float i)
    {
        gripTarget = i;
    }

    public void SetTrigger(float i)
    {
        triggerTarget = i;
    }
    #endregion

    void AnimateHand()
    {
        // Have item in hand, animate to hold gun animation when press grip
        if (isHandleItem)
        {
            // Not animate hand when select target is Spawn Gun
            if (XRRayInteractor.selectTarget.gameObject.tag != "SpawnGun")
            {
                // If grab when state is fist animation will reset fist value and set to hold gun animation
                if (animator.GetFloat(animatorFistParam) == 1)
                {
                    animator.SetFloat(animatorFistParam, 0);
                    animator.SetFloat(animatorGripParam, 1);
                }
                else
                {
                    if (gripCurrent != gripTarget)
                    {
                        gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
                        animator.SetFloat(animatorGripParam, gripCurrent);
                    }

                    if (triggerCurrent != triggerTarget)
                    {
                        triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * speed);
                        animator.SetFloat(animatorTriggerParam, triggerCurrent);
                    }
                }
            }

        }
        // Not have item in hand, animate to fist animation when press grip
        else
        {
            if (gripCurrent != gripTarget)
            {
                gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
                animator.SetFloat(animatorFistParam, gripCurrent);
            }
        }
    }
}
