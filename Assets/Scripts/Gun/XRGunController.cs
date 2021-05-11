using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGunController : MonoBehaviour
{
    private InteractCircleController InteractCircleController;
    private GunController gunController;
    private UIGunController UIGunController;
    [HideInInspector] public GrabChecker grabChecker;
    private Transform interactorHand;

    void Awake()
    {
        grabChecker = GetComponentInParent<GrabChecker>();
        gunController = GetComponent<GunController>();
        InteractCircleController = GetComponent<InteractCircleController>();
        UIGunController = GetComponentInChildren<UIGunController>();
    }

    void Start()
    {
        // Ray over object
        grabChecker.firstHoverEntered.AddListener(HoverEnter);

        // Ray out of object                                        
        grabChecker.lastHoverExited.AddListener(HoverExit);

        // Press grip to grab                                            
        grabChecker.selectEntered.AddListener(SelectEnter);

        // Un press grip to drop                                           
        grabChecker.selectExited.AddListener(SelectExit);

        // Press trigger                                              
        grabChecker.activated.AddListener(Activate);

        grabChecker.deactivated.AddListener(Deactivated);
    }

    private void Update()
    {
        ShowIntreactCircle();
    }

    void ShowIntreactCircle()
    {
        // Show interact circle when drop to ground and not in parent
        // Bug !! it will show 2 circle (left, right) when gun is stand with vertical /////////////////////////////////////////////////////
        if (!grabChecker.isSelected && transform.parent.position.y < 0.5f && transform.parent.parent == null)
        {
            InteractCircleController.ShowInteractCircle(true);
        }
    }

    public bool isAutoGun;

    private void Activate(ActivateEventArgs args)
    {
        if (GameManager.Instance.autoGun)
        {
            if (!isAutoGun)
            {
                isAutoGun = true;
                InvokeRepeating(nameof(Shooting), 0.01f, GameManager.Instance.autoGunFireRate_default);
            }
        }
        else
        {
            Shooting();
        }
    }

    private void Deactivated(DeactivateEventArgs args)
    {
        DisableAutoGun();
    }

    void Shooting()
    {
        gunController.ShootingBullet();


        if (gunController.countBullet > 0)
        {
            AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].shoot, gameObject.transform.position, AudioManager.Instance.shootVol);
        }
        else
        {
            AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].noAmmo, gameObject.transform.position, AudioManager.Instance.noAmmoVol);
        }
    }

    private void HoverEnter(HoverEnterEventArgs args)
    {
        InteractCircleController.SetAnimationInteract(true);
        gunController.SetRespondTime(false);
    }

    private void HoverExit(HoverExitEventArgs args)
    {
        InteractCircleController.SetAnimationInteract(false);
        gunController.SetRespondTime(true);
    }

    private void SelectEnter(SelectEnterEventArgs args)
    {
        // Disable sphere collider for that use grab in spawn point only
        transform.parent.GetComponent<GrabChecker>().colliders[1].enabled = false;

        // Show text of bullet
        UIGunController.textCount.gameObject.SetActive(true);
        gunController.UpdateTextBullet();
        // Show model of gun
        transform.parent.GetChild(0).gameObject.SetActive(true);

        // Hide interact circle
        InteractCircleController.ShowInteractCircle(false);

        // Set have respond with gun
        gunController.SetRespondTime(false);

        // Set is trigger for gun can trigger with hand to reload bullet
        transform.parent.GetComponent<GrabChecker>().colliders[0].isTrigger = true;

        // Set parent to hand
        interactorHand = args.interactor.gameObject.transform;
        Invoke("SetHandParent", 0.5f);

        AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].reload, args.interactor.gameObject.transform.position, AudioManager.Instance.reloadVol);

        GameManager.Instance.QuestCollect(1);
    }

    private void SelectExit(SelectExitEventArgs args)
    {
        // When drop set gun to not in hand parent
        transform.parent.SetParent(null);
        SetLayer(6);

        // Enable collider when drop the gun for have collider to detect with ground
        transform.parent.GetComponent<GrabChecker>().colliders[0].enabled = true;

        GetComponentInParent<Rigidbody>().isKinematic = false;

        // Hide text of gun
        UIGunController.textCount.gameObject.SetActive(false);

        // Start to counting time for destroy
        gunController.SetRespondTime(true);

        DisableAutoGun();
    }

    void DisableAutoGun()
    {
        if (isAutoGun)
        {
            isAutoGun = false;
            CancelInvoke();
        }
    }

    void SetHandParent()
    {
        if (grabChecker.isSelected)
        {
            transform.parent.SetParent(interactorHand);
            //transform.parent.gameObject.layer = 5;

            SetLayer(5);

            interactorHand = null;
        }
    }


    void SetLayer(int intlayer)
    {
        Transform[] skinGun = transform.parent.GetChild(0).GetComponentsInChildren<Transform>();

        for (int i = 0; i < skinGun.Length; i++)
        {
            skinGun[i].gameObject.layer = intlayer;
        }

        Transform[] uiCGM = transform.parent.GetChild(0).GetChild(2).GetChild(0).GetComponentsInChildren<Transform>();

        for (int i = 0; i < uiCGM.Length; i++)
        {
            uiCGM[i].gameObject.layer = intlayer;
        }

        Transform uiCount = transform.Find("UI").GetComponentInChildren<Transform>();
        Transform uiCrop = uiCount.GetComponentInChildren<Transform>();

        uiCount.gameObject.layer = intlayer;
        uiCrop.gameObject.layer = intlayer;

    }
}
