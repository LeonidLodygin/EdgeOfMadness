using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


public class ChaseBehaviour : StateMachineBehaviour
{
    NavMeshAgent agent; //���
    Transform player; //�����
    float chaseRange = 17;  // ������������ ���������� �� ������ ������������� ������
    float attackRange = 10; // ����������, �� ������� ���������� �����
    float longChaseRange = 25; //���������� ��� ������ ������������� ������ �� �����

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
        RaycastHit hit;

        // ��� ������� �����, � ������� ����������� ������� ������������
        // ���� ���� �������� ������������ � �������� �������� ��������� (chaseRange), �� ������ � ������������
        // ����� �������� � ���������� hit
        if (Physics.BoxCast(animator.transform.position, new Vector3(7f, 7f, 7f) / 2f, animator.transform.forward, out hit, Quaternion.identity, chaseRange))
        {
            //Debug.Log(hit.transform.name);

            // ���� ������ ���������� � ��� �� �����, �������� ���� �������������
            if (!hit.transform.CompareTag("Player"))
            {
                if (!DetectSound(distance, animator))
                {
                    // ���� ������������ �� ���������, �������� ���� �������������
                    animator.SetBool("IsChasing", false);
                }
            }
        }
        else
        {
            if (!DetectSound(distance, animator))
            {
                // ���� ������������ �� ���������, �������� ���� �������������
                animator.SetBool("IsChasing", false);
            }
        }
    }

    public bool DetectSound(float distance, Animator animator)
    {
        //��� ����� ���������� ����������� �� �����
        if (distance < longChaseRange)
        {
            bool isRunning = Keyboard.current[Key.LeftShift].isPressed;
            bool isMovingForward_W = Keyboard.current[Key.W].isPressed;
            bool isMovingForward_Arrow = Keyboard.current[Key.UpArrow].isPressed;
            if (isRunning && (isMovingForward_W || isMovingForward_Arrow))
            {
                return true;
            }
        }
        return false;
    }

    // ����� ��� ������ �� ��������� ������
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);

        //������������� �������� ���� 
        agent.speed = 2;
    }
}
