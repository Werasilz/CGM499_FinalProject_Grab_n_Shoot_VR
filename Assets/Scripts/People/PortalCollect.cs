using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCollect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PeopleArea"))
        {
            other.transform.GetComponentInParent<RagdollScript>().isEnterPortal = true;
            Destroy(other.transform.parent.gameObject);
            SpawnPeopleManager.Instance.peopleOnField -= 1;
            SpawnPeopleManager.Instance.peopleEscape += 1;
            SpawnPeopleManager.Instance.stackEscape += 1;
            UserInterfaceManager.Instance.SetEscapeFill();
            AudioManager.Instance.XRPlayAudio(AudioManager.Instance.AudioClips[0].portalEnter, gameObject.transform.position, AudioManager.Instance.portalEnterVol);
            GameManager.Instance.QuestCollect(10);
            return;
        }
    }
}
