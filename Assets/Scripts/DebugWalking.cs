using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugWalking : MonoBehaviour
{
    int direction = 0;
    // Update is called once per frame
    void Update()
    {
        var agent = GetComponent<NavMeshAgent>();
        if (agent.remainingDistance < 0.01f)
        {
            switch (direction)
            {
                case 0:
                    agent.SetDestination(transform.position + (Vector3.forward * 5));
                    direction = 1;
                    break;
                case 1:
                    agent.SetDestination(transform.position + (Vector3.left * 5));
                    direction = 2;
                    break;
                case 2:
                    agent.SetDestination(transform.position + (Vector3.back * 5));
                    direction = 3;
                    break;
                case 3:
                    agent.SetDestination(transform.position + (Vector3.right * 5));
                    direction = 0;
                    break;
            }
        }
    }
}
