using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField]
    protected float debugDrawRadius = 1.0f;
    [SerializeField]
    protected Color color;
    public virtual void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);
    }
}
