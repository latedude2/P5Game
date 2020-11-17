using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadbushTrigger : MonoBehaviour
{
    private float currentTime;
    public float waitTime;

    void Start()
    {
        currentTime = Time.time - waitTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        HurtEffect hurtableObject = other.GetComponent<HurtEffect>();
                
        if (hurtableObject != null)
        {
            if (currentTime + waitTime < Time.time)
            {
                currentTime = Time.time;
                hurtableObject.Hit();
            }
            
        }
    }
}
