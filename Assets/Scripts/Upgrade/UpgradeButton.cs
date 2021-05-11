using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UpgradeButton : MonoBehaviour
{
    private Animator animator;
    [HideInInspector] public ParticleSystem energyEffect;

    public bool[] type;
    public XRSimpleInteractable XRSimpleInteractable;

    private void Awake()
    {
        XRSimpleInteractable = GetComponent<XRSimpleInteractable>();
        animator = GetComponent<Animator>();
        energyEffect = GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        energyEffect.Stop();

        XRSimpleInteractable.firstHoverEntered.AddListener(HoverEnter);
        XRSimpleInteractable.lastHoverExited.AddListener(HoverExit);
        XRSimpleInteractable.selectEntered.AddListener(SelectEnter);
        XRSimpleInteractable.selectExited.AddListener(SelectExit);
    }

    private void Update()
    {
        if (GameManager.Instance.tokenCoins == 0)
        {
            energyEffect.Stop();
        }
    }

    private void HoverEnter(HoverEnterEventArgs args)
    {
        animator.SetBool("isHover", true);
    }

    private void HoverExit(HoverExitEventArgs args)
    {
        animator.SetBool("isHover", false);
    }

    private void SelectEnter(SelectEnterEventArgs args)
    {
        if (GameManager.Instance.tokenCoins > 0)
        {
            energyEffect.Play();
            GameManager.Instance.tokenCoins -= 1;

            // Max bullet
            if (type[0])
            {
                UpgradeManager.Instance.UpgradeBullet();
                GameManager.Instance.QuestCollect(7);
            }
            // Cooldown spawn gun
            else if (type[1])
            {
                UpgradeManager.Instance.UpgradeCooldown();
                GameManager.Instance.QuestCollect(8);
            }
            // Max HP
            else if (type[2])
            {
                UpgradeManager.Instance.UpgradeHealth();
                GameManager.Instance.QuestCollect(9);
            }
        }
    }

    private void SelectExit(SelectExitEventArgs args)
    {

    }
}
