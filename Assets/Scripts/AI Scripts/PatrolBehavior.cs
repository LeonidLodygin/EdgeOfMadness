using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


public class PatrolBehavior : StateMachineBehaviour
{
    float timer;
    //List <Transform> points = new List<Transform> (); //это удалится 
    NavMeshAgent agent;

    Transform player; //игрок 
    float chaseRange = 17;  // Максимальное расстояние до начала преследования игрока
    float longChaseRange = 25; //Расстояние для начала преследования игрока по звуку

    public float radiusPatrol = 25; //радиус патрулирования бота 
    public Vector3 centerPointPatrol; //центр сферы, откуда начинается патрулирование 

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
                   
        agent = animator.GetComponent<NavMeshAgent>();

        // Устанавливаем центральную точку патрулирования в начальную позицию бота
        centerPointPatrol = agent.transform.position;

        // Проверяем, активен ли и включен ли бот на навигационной сетке
        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            // Устанавливаем минимальное расстояние перед ботом перед остановкой
            agent.stoppingDistance = 1.0f; 
        }
        // Находим игровой объект с тегом "Player" и получаем его компонент Transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Добавлено условие проверки активности и нахождения на NavMesh
        if (agent.isActiveAndEnabled && agent.isOnNavMesh) 
        {
            // Проверяем, завершен ли путь до конечной точки
            if (agent.remainingDistance <= agent.stoppingDistance) //done with path
            {
                Vector3 point;

                // Генерируем случайную точку в пределах радиуса от центральной точки патрулирования
                if (RandomPoint(centerPointPatrol, radiusPatrol, out point)) 
                {
                    // Устанавливаем полученную случайную точку в качестве новой цели для бота
                    agent.SetDestination(point);
                }
            }
            
            
            timer += Time.deltaTime;
            
            //После 50 секунд бот делает перерыв в патрулировании
            if (timer > 50)
            {
                animator.SetBool("IsPatroling", false); 
            }
            
            //Аналогичные условия на смену расстояния, которые прописаны в скрипте ChaseBehavior 
            float distance = Vector3.Distance(animator.transform.position, player.position);

            //Создаем сфеерический луч, пускаемый прямо по направлению бота
            RaycastHit hit;

            // куб создает объем, в котором проверяется наличие столкновений
            // Если есть успешное столкновение в пределах заданной дистанции (chaseRange), то данные о столкновении
            // будут записаны в переменную hit
            if (Physics.BoxCast(animator.transform.position, new Vector3(7f, 7f, 7f) / 2f, animator.transform.forward, out hit, Quaternion.identity, chaseRange))
            {
                Debug.Log(hit.transform.name);
                //Если бот видит нас(т.е. луч попадает в Player и при этом подходящая дистанция, то начинаем преследование) 
                if (hit.transform.CompareTag("Player"))
                {
                    animator.SetBool("IsChasing", true);

                }
            }       
            //Это альфа реализация обнаружения по звуку
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
    // Функция для генерации случайной точки в пределах указанного радиуса патрулирования от заданной центральной точки
    // center: Центральная точка патрулирования
    // range: Радиус, в пределах которого должна быть сгенерирована случайная точка
    // result: Результирующая случайная точка на NavMesh (если успешно сгенерирована).
    // Возвращает true, если точка успешно сгенерирована и находится на NavMesh, иначе false.
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        // Генерируем случайную точку внутри сферы с центром в заданной центральной точке и радиусом
        Vector3 randomPoint = center + Random.insideUnitSphere * range;

        // Проверяем, находится ли сгенерированная точка на NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 3.0f, NavMesh.AllAreas))
        {
            // Если успешно, сохраняем позицию точки и возвращаем true
            result = hit.position;
            return true;
        }
        // Если точка не находится на NavMesh, возвращаем false
        result = Vector3.zero;
        return false;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

   
}
