using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactToTeleportTriggers : MonoBehaviour
{
    [SerializeField] private GameObject teleportTriggerParent;
    private List<TeleportTrigger> teleportTriggers = new List<TeleportTrigger>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in teleportTriggerParent.transform)
        {
            teleportTriggers.Add(child.GetComponent<TeleportTrigger>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TeleportTrigger teleportTrigger = other.GetComponent<TeleportTrigger>();
        if (teleportTrigger != null)
        {
            gameObject.transform.position = teleportTrigger.teleportDestination.transform.position;
        }
    }
}
