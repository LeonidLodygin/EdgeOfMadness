using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public LayerMask HidableLayers;
    public NavMeshAgent agent;

    private Animator animator; 
    public float HideSensitivity = 0;
    [Range(1, 10)]
    public float MinPlayerDistance = 5f;
    [Range(0, 5f)]
    public float UpdateFrequency = 0.5f;

    private Coroutine MovementCoroutine;
    private Collider[] Colliders = new Collider[10]; // массив коллайдеров за которыми можно спрятаться

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = agent.GetComponent<Animator>();
        Hide(player);
    }
    
    private IEnumerator Hide(Transform Target)
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateFrequency);
        while (true)
        {
            //массив коллайдеров отчищается каждый раз
            for (int i = 0; i < Colliders.Length; i++)
            {
                Colliders[i] = null;
            }

            int hits = Physics.OverlapSphereNonAlloc(agent.transform.position, 15f, Colliders, HidableLayers);

            int hitReduction = 0;
            for (int i = 0; i < hits; i++)
            {
                if (Vector3.Distance(Colliders[i].transform.position, Target.position) < MinPlayerDistance)
                {
                    Colliders[i] = null;
                    hitReduction++;
                }
            }
            hits -= hitReduction;

            System.Array.Sort(Colliders, ColliderArraySortComparer);

            for (int i = 0; i < hits; i++)
            {
                if (NavMesh.SamplePosition(Colliders[i].transform.position, out NavMeshHit hit, 2f, agent.areaMask))
                {
                    if (!NavMesh.FindClosestEdge(hit.position, out hit, agent.areaMask))
                    {
                        Debug.LogError($"Unable to find edge close to {hit.position}");
                    }

                    if (Vector3.Dot(hit.normal, (Target.position - hit.position).normalized) < HideSensitivity)
                    {
                        animator.SetBool("IsHiding", true);
                        agent.SetDestination(hit.position);
                        break;
                    }
                    else
                    {
                        // Since the previous spot wasn't facing "away" enough from teh target, we'll try on the other side of the object
                        if (NavMesh.SamplePosition(Colliders[i].transform.position - (Target.position - hit.position).normalized * 2, out NavMeshHit hit2, 2f, agent.areaMask))
                        {
                            if (!NavMesh.FindClosestEdge(hit2.position, out hit2, agent.areaMask))
                            {
                                Debug.LogError($"Unable to find edge close to {hit2.position} (second attempt)");
                            }

                            if (Vector3.Dot(hit2.normal, (Target.position - hit2.position).normalized) < HideSensitivity)
                            {
                                animator.SetBool("IsHiding", true);
                                agent.SetDestination(hit2.position);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError($"Unable to find NavMesh near object {Colliders[i].name} at {Colliders[i].transform.position}");
                }
            }
            yield return Wait;
        }
    }

    public int ColliderArraySortComparer(Collider A, Collider B)
    {
        if (A == null && B != null)
        {
            return 1;
        }
        else if (A != null && B == null)
        {
            return -1;
        }
        else if (A == null && B == null)
        {
            return 0;
        }
        else
        {
            return Vector3.Distance(agent.transform.position, A.transform.position).CompareTo(Vector3.Distance(agent.transform.position, B.transform.position));
        }
    }
}