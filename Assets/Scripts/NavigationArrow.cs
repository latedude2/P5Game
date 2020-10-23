﻿using System.Collections.Generic;
using UnityEngine;

public class NavigationArrow : MonoBehaviour, NavObject.TriggerListener
{

    // Angular speed in radians per sec.
    [SerializeField] private float rotationSpeed = 3f;
    
    [SerializeField] private GameObject navObjectParent;
    private List<NavObject> navObjects = new List<NavObject>();

    private int navObjectFollowed = 0;

    public void Triggered(NavObject navObject)
    {
        navObject.isFollowed = false;
        // if a player moved back
        if (navObjectFollowed > navObject.id)
        {
            navObjectFollowed = navObject.id;
        }
        navObjectFollowed++;
        CheckIfNavigationFinished();
        GetNewObject();
    }

    void Awake()
    {
        SetupNavObjects();
    }

    void Update()
    {
        CheckIfNavigationFinished();
        RotateArrow(GetNavObjectToFollow(navObjects[navObjectFollowed]).transform);
    }


    private void RotateArrow(Transform target)
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = target.position - transform.position;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0.0f);

        // Remap the direction, so the arrow doesn't rotate in vertical axis
        Vector3 remapWithoutY = new Vector3(newDirection.x, 0, newDirection.z);

        transform.rotation = Quaternion.LookRotation(remapWithoutY);
    }

    private NavObject GetNewObject()
    {
        CheckIfNavigationFinished();
        navObjects[navObjectFollowed].isFollowed = true;
        return navObjects[navObjectFollowed];
    }

    private NavObject GetNavObjectToFollow(NavObject navObject)
    {
        if (navObject.isFollowed)
        {
            return navObject;
        }
        //if the current object is not followed anymore, take the first one
        navObjectFollowed = 0;
        return GetNewObject();
    }

    private void SetupNavObjects()
    {
        int iterator = 0;
        foreach (Transform child in navObjectParent.transform)
        {
            NavObject navObject = child.gameObject.GetComponent<NavObject>();
            navObject.id = iterator;
            navObject.SetListener = this;
            navObjects.Add(navObject);
            iterator++;
        }
        navObjects.Reverse();
    }

    private void CheckIfNavigationFinished()
    {
        if (navObjectFollowed >= navObjects.Count || navObjectFollowed < 0)
        {
            // do whatever when the navigation was finished
            navObjectFollowed = 0;
        }
    }
}
