using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : StateMachineBehaviour
{
    NavMeshAgent agent; //���
    Transform player; //�����
    float chaseRange = 17;  // ������������ ���������� �� ������ ������������� ������
    float attackRange = 10; // ����������, �� ������� ���������� �����

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // �������� ��������� NavMeshAgent, ������������� �� ���������� ���������� ����
        agent = animator.GetComponent<NavMeshAgent>();

        //����������� �������� ���� �� ����� ������������� 
        agent.speed = 3;

        // ������� ������� ������ � ����� "Player" � �������� ��� ��������� Transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // �������������, ��� ��� ������ ��������� �� ������� 
        agent.SetDestination(player.position);
        //��������� ���������� ����� �������� ���� � �������� ������
        float distance = Vector3.Distance(animator.transform.position, player.position);

        // ���������, ��������� �� ����� � �������� ��������� �����,
        // ���� ��, ������������� ��������� ����� � true
        if (distance < attackRange) 
        {
            animator.SetBool("IsAttacking", true);
        }

        // ���������, ��������� �� ����� � �������� ��������� �������������,
        // ���� ���, ������������� ��������� ������������� � false
        if (distance > chaseRange)
        {
            animator.SetBool("IsChasing", false);
        }
    }

    // ����� ��� ������ �� ��������� ������
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);

        //������������� �������� ���� 
        agent.speed = 2;
    }
}