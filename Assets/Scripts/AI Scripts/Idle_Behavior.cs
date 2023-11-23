using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Idle_Behavior : StateMachineBehaviour
{
    float timer; // ���������� ��� ������������ �������
    Transform player; //����� 
    float chaseRange = 17; // ������������ ���������� �� ������ ������������� ������

    float longChaseRange = 25; //���� ����� ��������� �� ������ ���������� � ������������ 
    // � ������� SHIFT, �� ��� ������������� ��� ��� (�.�. ����������� �� �����) 

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0; // ����� ������� �� ���� ��� ����� � ���������

        // ������� ������� ������ � ����� "Player" � �������� ��� ��������� Transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ����������� ������ �� ��������� ����� � ������� ���������� �����
        timer += Time.deltaTime;

        //��� �������� �������������� ����� 2 �������
        if (timer > 2)
        { animator.SetBool("IsPatroling", true); }

        // ��������� ���������� ����� �������� ���� � �������� ������
        float distance = Vector3.Distance(animator.transform.position, player.position);

        RaycastHit hit;

        // ��� ������� �����, � ������� ����������� ������� ������������
        // ���� ���� �������� ������������ � �������� �������� ��������� (chaseRange), �� ������ � ������������
        // ����� �������� � ���������� hit
        if (Physics.BoxCast(animator.transform.position, new Vector3(7f, 7f, 7f) / 2f, animator.transform.forward, out hit, Quaternion.identity, chaseRange))
        {
            Debug.Log(hit.transform.name);
            //���� ��� ����� ���(�.�. ��� �������� � Player � ��� ���� ���������� ���������, �� �������� �������������) 
            if (hit.transform.CompareTag("Player"))
            {
                animator.SetBool("IsChasing", true);

            }

        }

        // ���������, ��������� �� ����� � �������� ��������� ��� ������������� �� ����
        if (distance < longChaseRange)
        {
            // ���������, ������ �� ������� "Shift" (���), � ������������ ������ �� ������� �������� ������
            bool isRunning = Keyboard.current[Key.LeftShift].isPressed;
            bool isMovingForward_W = Keyboard.current[Key.W].isPressed;
            bool isMovingForward_Arrow = Keyboard.current[Key.UpArrow].isPressed;

            // ���� ��� ����������� � ������������ ������ ������� �������� ������,
            // ������������� ��������� ������������� � true
            if (isRunning && (isMovingForward_W || isMovingForward_Arrow))
            {
                animator.SetBool("IsChasing", true);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }


}
