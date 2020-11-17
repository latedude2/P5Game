using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadbushTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject hurtableObject = other.GetComponent<HurtEffect>();

        if (hurtableObject != null)
        {
            hurtableObject.Hit();
        }
    }
}
