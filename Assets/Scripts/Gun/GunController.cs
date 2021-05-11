using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    #region Components
    private Animator animator;
    private XRGunController XRGunController;
    [HideInInspector] public UIGunController UIGunController;
    public GameObject popupDamage;
    public GameObject popupHeadShot;
    public GameObject muzzleFlash;
    public GameObject bulletEject;
    #endregion

    #region Shooting
    public GameObject bullet;
    private float fireRate = 0.0f;
    private float nextFire = 0.0f;

    // Distance for raycast from gun
    private float weaponRange = 500f;
    #endregion

    #region Gun Value
    // Max bullet in Magazine
    [SerializeField] public int maxMagazine;

    // Count of bullet                                                   
    [HideInInspector] public int countBullet;
    private bool activeRespond;

    // Wait until destroy
    private float respondTime;

    // Check gun is enter hand area
    public bool isEnterHand;

    private bool isDrop;
    #endregion

    public MeshRenderer[] skinGun;

    void Awake()
    {
        skinGun = transform.parent.GetChild(0).GetComponentsInChildren<MeshRenderer>();
        animator = GetComponentInParent<Animator>();
        XRGunController = GetComponent<XRGunController>();
        UIGunController = GetComponentInChildren<UIGunController>();
    }

    private void Start()
    {
        respondTime = GameManager.Instance.gunRespondTime_default;
    }

    private void OnEnable()
    {
        UpdateMaxBullet();
        UpdateSkin();
    }

    private void Update()
    {
        ActivateRespondTime();

        if (transform.parent.position.y < 0.05f && !isDrop)
        {
            isDrop = true;
            AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].gunDrop, transform.position, AudioManager.Instance.gunDropVol);
        }

        if (transform.parent.position.y > 0.1f && isDrop)
        {
            isDrop = false;
        }
    }

    public void UpdateSkin()
    {
        if (GameManager.Instance.autoGun)
        {
            ChangeSkin(1);
        }
        else
        {
            ChangeSkin(0);
        }
    }

    public void ChangeSkin(int skinID)
    {
        for (int i = 0; i < skinGun.Length; i++)
        {
            skinGun[i].material = ReferenceManager.Instance.skinGun[skinID];
        }
    }

    public void UpdateMaxBullet()
    {
        maxMagazine = GameManager.Instance.maxBullet_default + UpgradeManager.Instance.bulletTemp;
        countBullet = maxMagazine;
    }

    public void UpdateTextBullet()
    {
        UIGunController.textCount.text = countBullet.ToString();
    }

    #region Responding to Item
    public void SetRespondTime(bool value)
    {
        // When not have respond to this object set to activeRespond
        if (value)
        {
            activeRespond = true;
        }
        // When have respond set to false and set time to full until the value will true
        else
        {
            activeRespond = false;
            respondTime = GameManager.Instance.gunRespondTime_default;
        }
    }

    void ActivateRespondTime()
    {
        // Countdown time
        if (activeRespond)
        {
            if (!XRGunController.grabChecker.isSelected && !XRGunController.grabChecker.isHovered)
            {
                respondTime = respondTime - Time.deltaTime;
            }
        }

        // No respond to this object then destroy gameobject
        if (respondTime <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
    #endregion

    public void ShootingBullet()
    {
        if (Time.time > nextFire && countBullet > 0)
        {
            //sound can shoot
            Fire();
        }
        else
        {
            //sound no more bullet
        }
    }

    public void ReloadBullet()
    {
        // Reload on enter hand only 
        if (isEnterHand)
        {
            UpdateMaxBullet();
            UpdateTextBullet();
            UpdateSkin();
            UIGunController.SetTextColor();
            GameManager.Instance.QuestCollect(3);
        }
    }

    public void Fire()
    {
        animator.SetTrigger("GunTrigger");
        nextFire = Time.time + fireRate;

        // Effect
        muzzleFlash.GetComponent<ParticleSystem>().Play();
        bulletEject.GetComponent<ParticleSystem>().Play();

        // Bullet
        Instantiate(bullet, transform.position, transform.rotation);
        countBullet -= 1;

        // Text of bullet
        UIGunController.SetTextColor();
        UIGunController.textCount.text = countBullet.ToString();
        UIGunController.SetScale = true;

        UIGunController.GetComponent<Animator>().SetTrigger("isShoot");


        #region RayCast
        Vector3 rayOrigin = transform.position;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, transform.forward, out hit, weaponRange))
        {
            // Spawn Effect to raycast point
            if (hit.collider.gameObject.layer != 5 && hit.collider.gameObject.layer != 6)
            {
                SpawnDecal(hit, ReferenceManager.Instance.bulletShotEffect);
            }

            if (hit.collider.gameObject.layer == 7)
            {
                //AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].hitEnvironment, hit.collider.transform.position);
            }

            #region NPC
            // 1 hit kill when headshot
            if (hit.collider.CompareTag("CriticalPoint"))
            {
                if (hit.collider.GetComponentInParent<EnemyController>().hpEnemy > 0)
                {
                    SpawnPopup(hit, popupHeadShot);
                    SpawnDecal(hit, ReferenceManager.Instance.headShotEffect);
                    AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].headShot, hit.collider.transform.position, AudioManager.Instance.headShotVol);
                    PlayerManager.Instance.headShotCount += 1;
                    GameManager.Instance.scorePoints += GameManager.Instance.score_HeadShot;
                    UserInterfaceManager.Instance.SetScorePtsText();
                    hit.collider.GetComponentInParent<EnemyController>().hpEnemy -= GameManager.Instance.damage_HeadShot;
                    UserInterfaceManager.Instance.SetHeadShotFill();

                    GameManager.Instance.QuestCollect(6);
                }
                else
                {
                    SpawnDecal(hit, ReferenceManager.Instance.headShotEffect);
                    AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].hitBody, hit.collider.transform.position, AudioManager.Instance.hitBodyVol);
                }
            }

            // Decrease hp every hit until die
            if (hit.collider.CompareTag("Enemy"))
            {
                if (hit.collider.GetComponentInParent<EnemyController>().hpEnemy > 0)
                {
                    SpawnPopup(hit, popupDamage);
                    SpawnDecal(hit, ReferenceManager.Instance.bloodEffect);
                    AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].hitBody, hit.collider.transform.position, AudioManager.Instance.hitBodyVol);
                    GameManager.Instance.scorePoints += GameManager.Instance.score_BodyShot;
                    UserInterfaceManager.Instance.SetScorePtsText();
                    hit.collider.GetComponentInParent<EnemyController>().hpEnemy -= GameManager.Instance.damage_Body;
                }
                else
                {
                    SpawnDecal(hit, ReferenceManager.Instance.bloodEffect);
                    AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].hitBody, hit.collider.transform.position, AudioManager.Instance.hitBodyVol);
                }
            }

            // Decrease hp every hit until die
            if (hit.collider.CompareTag("People"))
            {
                if (hit.collider.GetComponentInParent<PeopleController>().hpPeople > 0)
                {
                    SpawnPopup(hit, popupDamage);
                    SpawnDecal(hit, ReferenceManager.Instance.bloodEffect);
                    GameManager.Instance.scorePoints -= GameManager.Instance.score_PeopleShot;
                    UserInterfaceManager.Instance.SetScorePtsText();
                    hit.collider.GetComponentInParent<PeopleController>().hpPeople -= GameManager.Instance.damage_Body;
                }
                else
                {
                    SpawnDecal(hit, ReferenceManager.Instance.bloodEffect);
                    AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].hitBody, hit.collider.transform.position, AudioManager.Instance.hitBodyVol);
                }
            }

            if (hit.collider.CompareTag("Boss"))
            {
                if (hit.collider.GetComponentInParent<BossController>().hpBoss > 0)
                {
                    if (hit.collider.GetComponentInParent<BossController>().hpBoss > 250)
                    {
                        SpawnDecal(hit, ReferenceManager.Instance.stoneEffect);
                    }
                    else
                    {
                        SpawnDecal(hit, ReferenceManager.Instance.explosionSmallStone);
                    }

                    GameManager.Instance.scorePoints += GameManager.Instance.score_BossBodyShot;
                    UserInterfaceManager.Instance.SetScorePtsText();
                    hit.collider.GetComponentInParent<BossController>().hpBoss -= GameManager.Instance.damage_Body;
                    hit.collider.GetComponentInParent<BossController>().healthBar.SetScaleBar();
                }
                else
                {
                    SpawnDecal(hit, ReferenceManager.Instance.explosionSmallStone);
                }

                AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].meteorHit, hit.collider.transform.position, AudioManager.Instance.meteorHitVol);
            }
            #endregion

            #region Object
            if (hit.collider.CompareTag("Meteor"))
            {
                if (hit.collider.GetComponent<Meteor>().HP > 0)
                {
                    hit.collider.GetComponent<Meteor>().HP -= 1;
                    SpawnDecal(hit, ReferenceManager.Instance.stoneEffect);
                    AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].meteorHit, hit.collider.transform.position, AudioManager.Instance.meteorHitVol);
                }
                else
                {
                    AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].meteorSmallExplosion, hit.collider.transform.position, AudioManager.Instance.meteorSmallExplosionVol);
                    GameObject spawnedDecal = GameObject.Instantiate(ReferenceManager.Instance.explosionMeteorEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(hit.collider.gameObject);
                }
            }

            if (hit.collider.CompareTag("UpgradeButton"))
            {
                if (GameManager.Instance.tokenCoins > 0)
                {
                    hit.collider.GetComponent<UpgradeButton>().energyEffect.Play();
                    GameManager.Instance.tokenCoins -= 1;

                    // Max bullet
                    if (hit.collider.GetComponent<UpgradeButton>().type[0])
                    {
                        UpgradeManager.Instance.UpgradeBullet();
                    }
                    // Cooldown spawn gun

                    if (hit.collider.GetComponent<UpgradeButton>().type[1])
                    {
                        UpgradeManager.Instance.UpgradeCooldown();

                    }
                    // Max HP
                    if (hit.collider.GetComponent<UpgradeButton>().type[2])
                    {
                        UpgradeManager.Instance.UpgradeHealth();
                    }
                }
            }

            if (hit.collider.CompareTag("Bomb"))
            {
                hit.collider.gameObject.GetComponentInParent<BombController>().StartBomb();
                AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].bomb, hit.collider.transform.position, AudioManager.Instance.bombVol);
                GameManager.Instance.QuestCollect(4);
            }
            #endregion
        }
    }
    #endregion

    public void SpawnDecal(RaycastHit hit, GameObject prefab)
    {
        GameObject spawnedDecal = GameObject.Instantiate(prefab, hit.point, Quaternion.LookRotation(hit.normal));
        spawnedDecal.transform.SetParent(hit.collider.transform);
    }

    public void SpawnPopup(RaycastHit hit, GameObject prefab)
    {
        GameObject spawnPopup = Instantiate(prefab, hit.point, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Reload when enter hand area
        if (other.gameObject.CompareTag("HandArea") && XRGunController.grabChecker.isSelected)
        {
            // isEnterHand must false for set it to true 
            // and can reload bullet then disable collider
            if (!isEnterHand)
            {
                isEnterHand = true;
                ReloadBullet();


                // Disable collider when grab for not trigger with another hand
                XRGunController.grabChecker.colliders[0].enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When gun exit handarea set isEnterHand to false
        // that can use function in OnTriggerEnter again for reload
        // and set isTrigger to false that player can grab gun when
        // gun exit handarea only 
        if (other.gameObject.CompareTag("HandArea"))
        {
            isEnterHand = false;
            XRGunController.grabChecker.colliders[0].isTrigger = false;
        }
    }
}
