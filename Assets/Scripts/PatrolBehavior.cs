using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehavior : StateMachineBehaviour
{
    float timer;
    List <Transform> points = new List<Transform> ();
    NavMeshAgent agent;

    Transform player;
    float chaseRange = 25;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        Transform pointsobject = GameObject.FindGameObjectWithTag("Points").transform;
        foreach (Transform t in pointsobject)
        {
            points.Add(t);
        }
        agent = animator.GetComponent<NavMeshAgent>();

        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.stoppingDistance = 1.0f; 
            agent.SetDestination(points[0].position);
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.isActiveAndEnabled && agent.isOnNavMesh) // Добавлено условие проверки активности и нахождения на NavMesh
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.SetDestination(points[Random.Range(0, points.Count)].position);
            }
            timer += Time.deltaTime;
            if (timer > 50)
            {
                animator.SetBool("IsPatroling", false); 
            }
            float distance = Vector3.Distance(animator.transform.position, player.position);
            if (distance < chaseRange) { 
                animator.SetBool("IsChasing", true); 
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

   
}
