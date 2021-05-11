using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownGunSpawn : MonoBehaviour
{
    [HideInInspector] public Image interactCircle;
    private float cooldownTime;

    // Is ready for grab when cooldown is finish, can't grab while cooldown counting
    public bool isReadyGrab;

    void Awake()
    {
        interactCircle = transform.GetChild(0).Find("Interact Circle").gameObject.GetComponentInChildren<Image>();
    }

    private void Update()
    {
        CooldownFinish();
        CountingCooldown();
    }

    public void SetCooldown()
    {
        isReadyGrab = false;
        interactCircle.fillAmount = 0;
        cooldownTime = GameManager.Instance.cooldownTime_default - UpgradeManager.Instance.cooldownTemp;
    }

    public void CountingCooldown()
    {
        if (!isReadyGrab)
        {
            interactCircle.fillAmount += Time.deltaTime / cooldownTime;
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(0).GetChild(0).GetComponentInChildren<Animator>().SetBool("isHover", false);
        }
    }

    void CooldownFinish()
    {
        // Can grab when circle in full fill
        if (interactCircle.fillAmount == 1)
        {
            isReadyGrab = true;
            if (transform.childCount >= 2)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
}
