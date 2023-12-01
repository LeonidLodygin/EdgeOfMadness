using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Behaivior : StateMachineBehaviour
{
    float attackRange = 10; //рассто€ние, начина€ с которого бот начинает атаку
    Transform player; //Ќаш главный герой 

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Ќаходим игровой объект с тегом "Player" и получаем его компонент Transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Ќаправл€ем взгл€д бота на игрока
        animator.transform.LookAt(player);
        //бот начинает заваливатьс€ при приближении к нему, так что мы замораживаем его положение
        animator.transform.eulerAngles = new Vector3(0, animator.transform.eulerAngles.y, 0); 
        // ¬ычисл€ем рассто€ние между позицией бота и позицией игрока
        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance > attackRange) 
        {
            // ≈сли рассто€ние больше attackRange, устанавливаем состо€ние атаки в false
            animator.SetBool("IsAttacking", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
