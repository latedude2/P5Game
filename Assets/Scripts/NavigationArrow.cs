using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationArrow : MonoBehaviour
{

    [SerializeField] private List<NavObject> navObjects;

    private int navObjectFollowed = 0;

    void Start()
    {
        // when the script starts, the first navObject is set to be followed
        navObjects[0].isFollowed = true;
    }

    void Update()
    {
        NavObject navObject = GetNavObjectToFollow(navObjects);
        //transform.LookAt(navObject.transform);
        CheckDistance(navObject);
    }

    private void CheckDistance(NavObject navObject)
    {
        float distance = Vector3.Distance(transform.position, navObject.transform.position);
        if (distance < 10)
        {
            navObject.setChecked(true);
            IterateNew();
        }
    }

    private void IterateNew()
    {
        navObjectFollowed++;
        if (navObjects.Count == 0 || navObjects[navObjectFollowed] == null)
            return;

        navObjects[navObjectFollowed].isFollowed = true;
    }

    private NavObject GetNavObjectToFollow(List<NavObject> navObjects)
    {
        foreach (NavObject navObject in navObjects)
        {
            if (navObject.isFollowed)
                return navObject;
        }
        //if none of the navObjects are followed, take the first one
        return navObjects[0];
    }

    // Angular speed in radians per sec.
    public float rotationSpeed = 0.01f;
    private void LateUpdate()
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = navObjects[navObjectFollowed].transform.position - transform.position;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0.0f);

        Vector3 remapToY = new Vector3(newDirection.x, 0, newDirection.z); 

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(remapToY);
    }
}
