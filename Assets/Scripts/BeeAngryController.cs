using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class BeeAngryController : MonoBehaviour
{
    public Transform player;

    private NavMeshAgent agent;
    private int refreshTargetTimer = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

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
            refreshTargetTimer = 50;
        }

        //add timer
        if (dist <= 2)
        {
            BeeAttack();
        }
    }

    void BeeAttack()
    {
        //Set bee run attack animation here

        //player.GetComponent<HurtEffect>().Hit();
        Debug.Log("DEAD!");
    }
}
