using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : MonoBehaviour
{
    private BossController bossController;

    void Awake()
    {
        bossController = GetComponentInParent<BossController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.name == "BossWall")
        {
            bossController.SetRunAnimation(false);
            bossController.SetRoarAnimation();
            Invoke(nameof(RoarSound), 1.4f);
            Destroy(other.gameObject);
        }
    }

    void RoarSound()
    {
        bossController.RoarSound();
    }
}
