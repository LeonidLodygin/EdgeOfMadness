using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


public class PatrolBehavior : StateMachineBehaviour
{
    float timer;
    //List <Transform> points = new List<Transform> (); //��� �������� 
    NavMeshAgent agent;

    Transform player; //����� 
    float chaseRange = 17;  // ������������ ���������� �� ������ ������������� ������
    float longChaseRange = 25; //���������� ��� ������ ������������� ������ �� �����

    public float radiusPatrol = 25; //������ �������������� ���� 
    public Vector3 centerPointPatrol; //����� �����, ������ ���������� �������������� 

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
                   
        agent = animator.GetComponent<NavMeshAgent>();

        // ������������� ����������� ����� �������������� � ��������� ������� ����
        centerPointPatrol = agent.transform.position;

        // ���������, ������� �� � ������� �� ��� �� ������������� �����
        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            // ������������� ����������� ���������� ����� ����� ����� ����������
            agent.stoppingDistance = 1.0f; 
        }
        // ������� ������� ������ � ����� "Player" � �������� ��� ��������� Transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ��������� ������� �������� ���������� � ���������� �� NavMesh
        if (agent.isActiveAndEnabled && agent.isOnNavMesh) 
        {
            // ���������, �������� �� ���� �� �������� �����
            if (agent.remainingDistance <= agent.stoppingDistance) //done with path
            {
                Vector3 point;

                // ���������� ��������� ����� � �������� ������� �� ����������� ����� ��������������
                if (RandomPoint(centerPointPatrol, radiusPatrol, out point)) 
                {
                    // ������������� ���������� ��������� ����� � �������� ����� ���� ��� ����
                    agent.SetDestination(point);
                }
            }
            
            
            timer += Time.deltaTime;
            
            //����� 50 ������ ��� ������ ������� � ��������������
            if (timer > 50)
            {
                animator.SetBool("IsPatroling", false); 
            }
            
            //����������� ������� �� ����� ����������, ������� ��������� � ������� ChaseBehavior 
            float distance = Vector3.Distance(animator.transform.position, player.position);

            //������� ������������ ���, ��������� ����� �� ����������� ����
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
            //��� ����� ���������� ����������� �� �����
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
    // ������� ��� ��������� ��������� ����� � �������� ���������� ������� �������������� �� �������� ����������� �����
    // center: ����������� ����� ��������������
    // range: ������, � �������� �������� ������ ���� ������������� ��������� �����
    // result: �������������� ��������� ����� �� NavMesh (���� ������� �������������).
    // ���������� true, ���� ����� ������� ������������� � ��������� �� NavMesh, ����� false.
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        // ���������� ��������� ����� ������ ����� � ������� � �������� ����������� ����� � ��������
        Vector3 randomPoint = center + Random.insideUnitSphere * range;

        // ���������, ��������� �� ��������������� ����� �� NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 3.0f, NavMesh.AllAreas))
        {
            // ���� �������, ��������� ������� ����� � ���������� true
            result = hit.position;
            return true;
        }
        // ���� ����� �� ��������� �� NavMesh, ���������� false
        result = Vector3.zero;
        return false;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

   
}
