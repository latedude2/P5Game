using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationArrow : MonoBehaviour
{

    [SerializeField] private List<NavObject> navObjects;


    void Start()
    {
        // when the script starts, the first navObject is set to be followed
        navObjects[0].isFollowed = true;
    }

    void Update()
    {
        NavObject navObject = GetNavObjectToFollow(navObjects);
        transform.LookAt(navObject.transform);
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
}
