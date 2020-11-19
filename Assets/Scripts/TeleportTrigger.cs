using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    public GameObject teleportDestination;  //where the player is taken when hitting this trigger
    public bool shouldHurt = true;

    private void OnTriggerEnter(Collider other)
    {
        if (shouldHurt)
        {
            HurtEffect hurtableObject = other.GetComponent<HurtEffect>();

            if (hurtableObject != null)
            {
                hurtableObject.Hit(false);
            }
        }
    }
}