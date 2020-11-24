using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCullingTriggers : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CullingTrigger cullingTrigger = other.GetComponent<CullingTrigger>();
        if (cullingTrigger != null)
        {
            foreach (GameObject gameObject in cullingTrigger.gameObjectsToEnable)
            {
                gameObject.SetActive(true);
            }
            foreach (GameObject gameObject in cullingTrigger.gameObjectsToDisable)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
