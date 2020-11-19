﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class BeeAngryController : MonoBehaviour
{
    public Transform player;
    public float beeBaseSpeed;
    private float distanceBeeToPlayer;
    public int minDist = 2;
    public int minSpeed = 2;

    private NavMeshAgent agent;
    public Animator animator;
    private int refreshTargetTimer = 0;
    public int refreshTargetTimerLimit = 50;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        beeBaseSpeed = agent.speed;

        animator.Play("AngryFlight");

        agent.stoppingDistance = 2;
    }

    void Update()
    {
        distanceBeeToPlayer = Vector3.Distance(player.position, transform.position);
        SetBeeSpeed();

        FindNavTarget();

        BeeAttack();
    }

    void SetBeeSpeed()
    {
        //Calculation for bee relative speed. We can discuss what this should be.
        agent.speed = beeBaseSpeed * (distanceBeeToPlayer * 0.01f);
        if (agent.speed < minSpeed)
            agent.speed = minSpeed;
    }

    void FindNavTarget()
    {
        refreshTargetTimer -= 1;

        //we set destination for target to run less than every frame, cause it's heavy over longer distances
        if (refreshTargetTimer <= 0)
        {
            agent.destination = player.position;
            refreshTargetTimer = refreshTargetTimerLimit;
        }
    }

    void BeeAttack()
    {
        if (distanceBeeToPlayer <= minDist)
        {
            //Set bee run attack animation here
            animator.Play("Attack");

            //player.GetComponent<HurtEffect>().Hit();
            Debug.Log("DEAD!");
        }
    }
}