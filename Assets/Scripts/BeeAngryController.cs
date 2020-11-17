using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class BeeAngryController : MonoBehaviour
{
    public Transform player;
    public int minDist = 2;

    private NavMeshAgent agent;
    public Animator anim;
    private int refreshTargetTimer = 0;
    public int refreshTargetTimerLimit = 50;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        anim.Play("AngryFlight");

        agent.stoppingDistance = 2;
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        refreshTargetTimer -= 1;

        //Calculation for bee relative speed. We can discuss what this should be.
        agent.speed = 25 * (dist * 0.01f);
        if (agent.speed < 2)
            agent.speed = 2;

        //we set destination for target to run less than every frame, cause it's heavy over longer distances
        if (refreshTargetTimer <= 0)
        {
            agent.destination = player.position;
            refreshTargetTimer = refreshTargetTimerLimit;
        }

        //add timer
        if (dist <= minDist)
        {
            BeeAttack();
        }
    }

    void BeeAttack()
    {
        //Set bee run attack animation here
        anim.Play("Attack");

        //player.GetComponent<HurtEffect>().Hit();
        Debug.Log("DEAD!");
    }
}
