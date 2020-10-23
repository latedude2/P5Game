using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class BeeController : MonoBehaviour
{
    public Transform player;
    public List<Transform> navObjects = new List<Transform>();

    [SerializeField] private GameObject guidePointParent;
    public float beeSpookDistance = 5;
    private int destPoint = 0;
    private NavMeshAgent agent;

    void Start()
    {
        foreach (Transform child in guidePointParent.transform)
        {
            navObjects.Add(child);
        }
        navObjects.Reverse();

        agent = GetComponent<NavMeshAgent>();

        //Set first point as goal
        agent.destination = navObjects[destPoint].position;
    }

    void Update()
    {
        // Find distance to player
        float dist = Vector3.Distance(player.position, transform.position);

        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f && dist < beeSpookDistance)
            GotoNextPoint();
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (navObjects.Count == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = navObjects[destPoint].position;


        destPoint = (destPoint + 1);
    }

}