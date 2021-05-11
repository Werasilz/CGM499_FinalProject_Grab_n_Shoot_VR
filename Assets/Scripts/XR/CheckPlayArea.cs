using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CheckPlayArea : MonoBehaviour
{
    private TeleportationAnchor TeleportationAnchor;
    private GameObject[] warpPoint;                                                             // All warp point
    public GameObject[] nextPoint;                                                              // 2 warp point to teleport next to current area
    public int areaNumber;                                                                      // Number of this warp area
    private GameObject hightlight;

    private void Awake()
    {
        TeleportationAnchor = transform.parent.parent.GetComponent<TeleportationAnchor>();
        hightlight = transform.parent.Find("Hightlight").gameObject;
        warpPoint = GameObject.FindGameObjectsWithTag("WarpPoint");
    }

    private void Start()
    {
        if (PlayerManager.Instance.currentArea == 0)                                                            // Start area 
        {
            for (int i = 1; i < warpPoint.Length; i++)                                          // Hide all warp point 
            {
                warpPoint[i].SetActive(false);
            }
        }

        TeleportationAnchor.firstHoverEntered.AddListener(OnHoverEnter);
        TeleportationAnchor.lastHoverExited.AddListener(OnHoverExit);
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        hightlight.SetActive(true);
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        hightlight.SetActive(false);
    }

    /// <summary>
    /// Warp Point Node
    /// Node 0 > 1 2
    /// Node 1 > 3 4
    /// Node 2 > 5 6
    /// Node 3 > 7 8
    /// Node 4 > 9 10
    /// Node 5 > 11 12
    /// Node 6 > 13 14 
    /// </summary>
    /// <param name="Warp Point Node"></param>

    /// <summary>
    /// Player enter center area of warp point will show other warp point for teleport
    /// This will check player current area that set value when select warp point and teleport to that point
    /// compare with area number and set next point of current warp point to show when stay on center point or hide when exit
    /// </summary>
    /// <param name="Trigger Center Area with Player"></param>

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (PlayerManager.Instance.currentArea == areaNumber)
            {
                nextPoint[0].SetActive(true);
                nextPoint[1].SetActive(true);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (PlayerManager.Instance.currentArea == areaNumber)
            {
                nextPoint[0].SetActive(false);
                nextPoint[1].SetActive(false);
            }
        }
    }
}
