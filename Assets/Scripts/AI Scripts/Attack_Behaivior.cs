using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Behaivior : StateMachineBehaviour
{
    float attackRange = 10; //расстояние, начиная с которого бот начинает атаку
    Transform player; //Наш главный герой 

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Находим игровой объект с тегом "Player" и получаем его компонент Transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Направляем взгляд бота на игрока
        animator.transform.LookAt(player);

        // Вычисляем расстояние между позицией бота и позицией игрока
        float distance = Vector3.Distance(animator.transform.position, player.position);
    
        if (distance > attackRange) 
        {
            // Если расстояние больше attackRange, устанавливаем состояние атаки в false
            animator.SetBool("IsAttacking", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
