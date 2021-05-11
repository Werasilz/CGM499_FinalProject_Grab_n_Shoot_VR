using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalDestroy : MonoBehaviour
{
    // Time to appear in game before destroy
    private float lifeTimeNormal = 2.5f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(lifeTimeNormal);
        Destroy(gameObject);
    }
}
