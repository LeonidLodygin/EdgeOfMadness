using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Idle_Behavior : StateMachineBehaviour
{
    float timer; // ѕеременна€ дл€ отслеживани€ времени
    Transform player; //игрок 
    float chaseRange = 17; // ћаксимальное рассто€ние до начала преследовани€ игрока

    float longChaseRange = 25; //≈сли игрок находитс€ на данном рассто€нии и перемещаетс€ 
    // с помощью SHIFT, то это расцениваетс€ как бег (т.е. обнаружение по звуку) 

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0; // —брос таймера до нул€ при входе в состо€ние

        // Ќаходим игровой объект с тегом "Player" и получаем его компонент Transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ”величиваем таймер на прошедшее врем€ с момента последнего кадра
        timer += Time.deltaTime;

        //Ѕот начинает патрулирование через 2 секунды
        if (timer > 2)
        { animator.SetBool("IsPatroling", true); }

        // ¬ычисл€ем рассто€ние между позицией бота и позицией игрока
        float distance = Vector3.Distance(animator.transform.position, player.position);

        RaycastHit hit;

        // куб создает объем, в котором провер€етс€ наличие столкновений
        // ≈сли есть успешное столкновение в пределах заданной дистанции (chaseRange), то данные о столкновении
        // будут записаны в переменную hit
        if (Physics.BoxCast(animator.transform.position, new Vector3(7f, 7f, 7f) / 2f, animator.transform.forward, out hit, Quaternion.identity, chaseRange))
        {
            Debug.Log(hit.transform.name);
            //≈сли бот видит нас(т.е. луч попадает в Player и при этом подход€ща€ дистанци€, то начинаем преследование) 
            if (hit.transform.CompareTag("Player"))
            {
                animator.SetBool("IsChasing", true);

            }

        }

        // ѕровер€ем, находитс€ ли игрок в пределах дистанции дл€ распозновани€ по бегу
        if (distance < longChaseRange)
        {
            // ѕровер€ем, нажата ли клавиша "Shift" (бег), и одновременно нажата ли клавиша движени€ вперед
            bool isRunning = Keyboard.current[Key.LeftShift].isPressed;
            bool isMovingForward_W = Keyboard.current[Key.W].isPressed;
            bool isMovingForward_Arrow = Keyboard.current[Key.UpArrow].isPressed;

            // ≈сли бег активирован и одновременно нажата клавиша движени€ вперед,
            // устанавливаем состо€ние преследовани€ в true
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
