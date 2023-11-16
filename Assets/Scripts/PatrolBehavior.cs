using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


public class PatrolBehavior : StateMachineBehaviour
{
    float timer;
    List <Transform> points = new List<Transform> ();
    NavMeshAgent agent;

    Transform player;
    float chaseRange = 17;
    float longChaseRange = 25;

    public float range = 25;
    public Vector3 centerPoint;

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

        centerPoint = agent.transform.position;

        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.stoppingDistance = 1.0f; 
            //agent.SetDestination(points[0].position);
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.isActiveAndEnabled && agent.isOnNavMesh) // Добавлено условие проверки активности и нахождения на NavMesh
        {
            /*
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.SetDestination(points[Random.Range(0, points.Count)].position);
            }*/
            if (agent.remainingDistance <= agent.stoppingDistance) //done with path
            {
                Vector3 point;
                if (RandomPoint(centerPoint, range, out point)) //pass in our centre point and radius of area
                {
                    //Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                    agent.SetDestination(point);
                }
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

            if (distance < longChaseRange)
            {
                bool isRunning = Keyboard.current[Key.LeftShift].isPressed;
                bool isMovingForward_W = Keyboard.current[Key.W].isPressed;
                bool isMovingForward_Arrow = Keyboard.current[Key.UpArrow].isPressed;
                if (isRunning && (isMovingForward_W || isMovingForward_Arrow) ) 
                {
                    animator.SetBool("IsChasing", true);
                }
            }
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

   
}
