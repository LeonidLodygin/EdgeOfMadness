using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : StateMachineBehaviour
{
    NavMeshAgent agent; //бот
    Transform player; //игрок
    float chaseRange = 17;  // ћаксимальное рассто€ние до начала преследовани€ игрока
    float attackRange = 10; // –ассто€ние, на котором начинаетс€ атака

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ѕолучаем компонент NavMeshAgent, ответственный за управление навигацией бота
        agent = animator.GetComponent<NavMeshAgent>();

        //”величиваем скорость бота во врем€ преследовани€ 
        agent.speed = 3;

        // Ќаходим игровой объект с тегом "Player" и получаем его компонент Transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ”станавливаем, что бот должен двигатьс€ за игроком 
        agent.SetDestination(player.position);
        //¬ычисл€ем рассто€ние между позицией бота и позицией игрока
        float distance = Vector3.Distance(animator.transform.position, player.position);

        // ѕровер€ем, находитс€ ли игрок в пределах дистанции атаки,
        // если да, устанавливаем состо€ние атаки в true
        if (distance < attackRange) 
        {
            animator.SetBool("IsAttacking", true);
        }

        // ѕровер€ем, находитс€ ли игрок в пределах дистанции преследовани€,
        // если нет, устанавливаем состо€ние преследовани€ в false
        RaycastHit hit;

        // куб создает объем, в котором провер€етс€ наличие столкновений
        // ≈сли есть успешное столкновение в пределах заданной дистанции (chaseRange), то данные о столкновении
        // будут записаны в переменную hit
        if (Physics.BoxCast(animator.transform.position, new Vector3(7f, 7f, 7f) / 2f, animator.transform.forward, out hit, Quaternion.identity, chaseRange))
        {
            //Debug.Log(hit.transform.name);

            // ≈сли объект столкнулс€ и это не игрок, сбросить флаг преследовани€
            if (!hit.transform.CompareTag("Player"))
            {
                animator.SetBool("IsChasing", false);
            }
        }
        else
        {
            // ≈сли столкновение не произошло, сбросить флаг преследовани€
            animator.SetBool("IsChasing", false);
        }
    }

    // ћетод при выходе из состо€ни€ погони
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);

        //устанавливаем скорость бота 
        agent.speed = 2;
    }
}
