using UnityEngine;
using UnityEngine.AI;

public class Hide_Behaviour : StateMachineBehaviour
{
    Transform targetPlayer;
    NavMeshAgent navMeshAgent;
    public float minPlayerDistance = 100f;
    public float minObstacleHeight = 5.25f;
    public float updateFrequency = 0.1f;
    private Collider[] nearbyColliders = new Collider[10];
    private Vector3 playerInitialPosition;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        playerInitialPosition = targetPlayer.position; // Save player's position upon entering the state
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Clear the nearbyColliders array before each scan
        for (int i = 0; i < nearbyColliders.Length; i++)
        {
            nearbyColliders[i] = null;
        }

        int detectedHits = Physics.OverlapSphereNonAlloc(navMeshAgent.transform.position, 15f, nearbyColliders);

        // Exclude objects that are not suitable as hiding places
        int excludedHitsCount = 0;
        for (int i = 0; i < detectedHits; i++)
        {
            if (Vector3.Distance(nearbyColliders[i].transform.position, targetPlayer.position) < minPlayerDistance || nearbyColliders[i].bounds.size.y < minObstacleHeight)
            {
                nearbyColliders[i] = null;
                excludedHitsCount++;
            }
        }
        detectedHits -= excludedHitsCount;

        // Sort the nearbyColliders array to prioritize hiding places
        System.Array.Sort(nearbyColliders, ColliderArraySortComparer);

        // Attempt to find a suitable hiding place
        for (int i = 0; i < detectedHits; i++)
        {
            // Attempt to find the nearest point on the NavMesh to the object
            if (NavMesh.SamplePosition(nearbyColliders[i].transform.position, out NavMeshHit hit, 7f, navMeshAgent.areaMask))
            {
                if (nearbyColliders[i].gameObject.tag != "wall")
                {
                    // Attempt to find the nearest NavMesh edge to the object
                    if (!NavMesh.FindClosestEdge(hit.position, out hit, navMeshAgent.areaMask)) { }

                    // Check the direction of the edge relative to the vector to the target
                    if (Vector3.Dot(hit.normal, (targetPlayer.position - hit.position).normalized) < 0)
                    {
                        // Set the found point as the new destination for NavMeshAgent
                        animator.SetBool("IsHiding", true);
                        navMeshAgent.SetDestination(hit.position);
                        break;
                    }
                    else
                    {
                        // If the previous point is not suitable, try from the other side of the object
                        if (NavMesh.SamplePosition(nearbyColliders[i].transform.position - (targetPlayer.position - hit.position).normalized * 2, out NavMeshHit hit2, 15f, navMeshAgent.areaMask))
                        {
                            // Attempt to find the nearest NavMesh edge to the object (second attempt)
                            if (!NavMesh.FindClosestEdge(hit2.position, out hit2, navMeshAgent.areaMask))
                            {
                            }

                            // Check the direction of the edge relative to the vector to the target
                            if (Vector3.Dot(hit2.normal, (targetPlayer.position - hit2.position).normalized) < 0)
                            {
                                // Set the found point as the new destination for NavMeshAgent
                                animator.SetBool("IsHiding", true);
                                navMeshAgent.SetDestination(hit2.position);
                                break;
                            }
                        }
                    }
                }
            }
        }

        if (!animator.GetBool("IsHiding"))
        {
            animator.SetBool("IsAttacking", true);
        }
    }

    public int ColliderArraySortComparer(Collider colliderA, Collider colliderB)
    {
        if (colliderA == null && colliderB != null)
        {
            return 1;
        }
        else if (colliderA != null && colliderB == null)
        {
            return -1;
        }
        else if (colliderA == null && colliderB == null)
        {
            return 0;
        }
        else
        {
            return Vector3.Distance(navMeshAgent.transform.position, colliderA.transform.position).CompareTo(Vector3.Distance(navMeshAgent.transform.position, colliderB.transform.position));
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
