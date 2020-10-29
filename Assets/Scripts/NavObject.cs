﻿using System;
using UnityEngine;

public class NavObject : MonoBehaviour
{
    public bool isFollowed = false;

    [NonSerialized] public int id;

    public interface TriggerListener
    {
        void Triggered(NavObject navObject);
    };

    private TriggerListener triggerListener;
    public TriggerListener SetListener {set => triggerListener = value; }

    private void OnTriggerEnter(Collider other)
    {
        triggerListener.Triggered(this);
    }

    public float GetDistanceToTarget(Transform target)
    {
        return Vector3.Distance(target.position, transform.position);
    }
}