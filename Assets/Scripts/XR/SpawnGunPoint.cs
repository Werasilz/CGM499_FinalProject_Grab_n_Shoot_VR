using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpawnGunPoint : MonoBehaviour
{
    public GameObject gun;
    private Transform MainCamera;
    private XRSimpleInteractable XRSimpleInteractable;
    private CooldownGunSpawn CooldownGunSpawn;

    void Awake()
    {
        MainCamera = GameObject.Find("Main Camera").GetComponent<Transform>();
        XRSimpleInteractable = GetComponent<XRSimpleInteractable>();
        CooldownGunSpawn = GetComponent<CooldownGunSpawn>();
    }

    private void Start()
    {
        XRSimpleInteractable.firstHoverEntered.AddListener(OnHoverEnter);
        XRSimpleInteractable.lastHoverExited.AddListener(OnHoverExit);
    }

    void OnHoverEnter(HoverEnterEventArgs args)
    {
        SetAnimationInteractGameObject(true);
    }

    void OnHoverExit(HoverExitEventArgs args)
    {
        SetAnimationInteractGameObject(false);
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, MainCamera.position.y - 0.7f, transform.position.z);
    }

    private void Update()
    {
        SpawnGun();
    }

    void SetAnimationInteractGameObject(bool value)
    {
        if (CooldownGunSpawn.isReadyGrab)
        {
            transform.GetComponentInChildren<Animator>().SetBool("isHover", value);
            transform.GetChild(0).GetChild(0).GetComponentInChildren<Animator>().SetBool("isHover", value);
        }
        else
        {
            transform.GetComponentInChildren<Animator>().SetBool("isHover", false);
        }
    }

    public void SpawnGun()
    {
        // Spawn Gun when no child in spawn point
        if (gameObject.transform.childCount <= 1)
        {
            // Start cooldown for spawn new gun
            CooldownGunSpawn.SetCooldown();

            GameObject gunClone = Instantiate(gun, transform.position, transform.rotation);

            // Update Gun
            gunClone.GetComponentInChildren<GunController>().UpdateMaxBullet();
            gunClone.GetComponentInChildren<GunController>().UpdateTextBullet();
            gunClone.GetComponentInChildren<GunController>().UpdateSkin();

            // Gun Clone Setup
            gunClone.transform.name = "Pistol Gun";
            gunClone.transform.SetParent(gameObject.transform);
            gunClone.transform.rotation = Quaternion.Euler(0, -90, -90);

            gunClone.GetComponentInChildren<UIGunController>().textCount.gameObject.SetActive(false);
            // Disable box collider, use only sphere collider for grab
            gunClone.GetComponent<GrabChecker>().colliders[0].enabled = false;

            // Hide Interact circle
            gunClone.GetComponentInChildren<InteractCircleController>().ShowInteractCircle(false);

            // Hide model of gun
            gunClone.transform.GetChild(0).gameObject.SetActive(false);

            // For gun not drop by gravity
            gunClone.GetComponent<Rigidbody>().isKinematic = true;

        }

        // If have more 1 gun in child it will get out of spawn point
        /*if (gameObject.transform.childCount > 2)
        {
            gameObject.transform.GetChild(2).parent = null;
        }*/
    }
}
