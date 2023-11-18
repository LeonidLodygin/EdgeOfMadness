using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Behaivior : StateMachineBehaviour
{
    float attackRange = 10; //����������, ������� � �������� ��� �������� �����
    Transform player; //��� ������� ����� 

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ������� ������� ������ � ����� "Player" � �������� ��� ��������� Transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���������� ������ ���� �� ������
        animator.transform.LookAt(player);

        // ��������� ���������� ����� �������� ���� � �������� ������
        float distance = Vector3.Distance(animator.transform.position, player.position);
    
        if (distance > attackRange) 
        {
            // ���� ���������� ������ attackRange, ������������� ��������� ����� � false
            animator.SetBool("IsAttacking", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
