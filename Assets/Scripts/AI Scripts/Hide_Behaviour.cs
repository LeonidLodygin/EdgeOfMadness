using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

public class Hide_Behaviour : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;
    public float minPlayerDistance = 100f;
    public float minObstacleHeight = 5.25f;
    public float updateFrequency = 0.1f;
    private Collider[] colliders = new Collider[10];
    private Vector3 playerPosition; // 


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerPosition = player.position; // Сохраняем позицию игрока при входе в состояние

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Очистка массива colliders перед каждым сканированием
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i] = null;
        }


        int hits = Physics.OverlapSphereNonAlloc(agent.transform.position, 15f, colliders);

        // Исключение объектов, которые не подходят как места для спрятывания
        int hitReduction = 0;
        for (int i = 0; i < hits; i++)
        {
            if (Vector3.Distance(colliders[i].transform.position, player.position) < minPlayerDistance || colliders[i].bounds.size.y < minObstacleHeight)
            {
                colliders[i] = null;
                hitReduction++;
            }
        }
        hits -= hitReduction;

        // Сортировка массива Colliders для приоритизации мест для спрятывания
        System.Array.Sort(colliders, ColliderArraySortComparer);
        // Попытка найти подходящее место для спрятывания
        for (int i = 0; i < hits; i++)
        {
            // Попытка найти ближайшую точку на NavMesh к объекту
            if (NavMesh.SamplePosition(colliders[i].transform.position, out NavMeshHit hit, 7f, agent.areaMask))
            {
                if (colliders[i].gameObject.tag != "wall")
                {
                    // Попытка найти ближайший к объекту край NavMesh
                    if (!NavMesh.FindClosestEdge(hit.position, out hit, agent.areaMask))
                    {
                        Debug.LogError($"Unable to find edge close to {hit.position}");
                    }

                    // Проверка направления края относительно вектора к цели
                    if (Vector3.Dot(hit.normal, (player.position - hit.position).normalized) < 0)
                    {
                        // Установка найденной точки как новой цели для NavMeshAgent
                        animator.SetBool("IsHiding", true);
                        agent.SetDestination(hit.position);
                        break;
                    }
                    else
                    {
                        // Если предыдущая точка не подходит, пробуем с другой стороны объекта
                        if (NavMesh.SamplePosition(colliders[i].transform.position - (player.position - hit.position).normalized * 2, out NavMeshHit hit2, 15f, agent.areaMask))
                        {
                            // Попытка найти ближайший к объекту край NavMesh (вторая попытка)
                            if (!NavMesh.FindClosestEdge(hit2.position, out hit2, agent.areaMask))
                            {
                                Debug.LogError($"Unable to find edge close to {hit2.position} (second attempt)");
                            }

                            // Проверка направления края относительно вектора к цели
                            if (Vector3.Dot(hit2.normal, (player.position - hit2.position).normalized) < 0)
                            {
                                // Установка найденной точки как новой цели для NavMeshAgent
                                animator.SetBool("IsHiding", true);
                                agent.SetDestination(hit2.position);
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.LogError($"Unable to find NavMesh near object {colliders[i].name} at {colliders[i].transform.position}");
            }
        }

        if (!animator.GetBool("IsHiding"))
        {
            animator.SetBool("IsAttacking", true);
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

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }


}
