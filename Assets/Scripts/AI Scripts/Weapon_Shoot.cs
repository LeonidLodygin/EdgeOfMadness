using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class Weapon_Shoot : MonoBehaviour
{
    //public ParticleSystem _particleSystem; // Система частиц для визуального эффекта стрельбы

    //public float range = 100f; 
    
    int impactForce = 3000; // Сила для воздействия на игрока при выстреле в него

    Transform enemy; //бот
    Transform player; //игрок
    Animator enemyAnimator; //аниматор бота


    public VisualEffect muzzleFlash; // Эффект вспышки при стрельбе
    public Transform firePoint; // Точка, откуда "выпускаются" выстрелы

    private float nextTimeToFire; //Задержка между выстрелами 


    private void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy1").transform; 
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAnimator = enemy.GetComponent<Animator>();
    }
    
    
    void Update()
    {
        // Увеличиваем время до следующего выстрела
        nextTimeToFire += 1 * Time.deltaTime;

        // Проверяем, прошло ли достаточно времени с предыдущего выстрела и бот в состоянии атаки
        if (nextTimeToFire>1.2 && enemyAnimator.GetBool("IsAttacking"))
        {
            nextTimeToFire = 0;  // Сбрасываем время до следующего выстрела
            Shoot(); // Вызываем метод для выполнения выстрела
        }
    }

    // Функция для выполнения выстрела, включая воспроизведение эффекта вспышки,
    // определение попадания и причинения урона
    void Shoot()
    {
        // Воспроизводим эффект выстрела 
        muzzleFlash.Play();

        // Создаем луч, который представляет собой линию от позиции бота в направлении его вперед.
        RaycastHit hit;
        if (Physics.Raycast(enemy.transform.position, enemy.transform.forward, out hit))
        {
            //Debug.Log(hit.transform.name);

            // Получаем компонент Health объекта, в который попал луч
            Health target = hit.transform.GetComponent<Health>();

            // Если у объекта есть компонент Health, причиняем урон (эквивалентно, тому, если мы попали в игрока)
            if (target != null)
            {
                // Определяем уровень урона с использованием функции DetectionDemage
                float damage = DetectionDemage();

                // Передаем урон объекту Health
                target.TakeDamage(damage);
            }

            // Если объект имеет Rigidbody, добавляем силу в направлении, обратном нормали попадания
            if (hit.rigidbody != null)
            {
                // Добавляем силу в точке попадания
                hit.rigidbody.AddForceAtPosition(-hit.normal * impactForce, hit.point);
            }
        };
    }

    // Функция для определения уровня урона в зависимости от случайной области попадания
    // Возвращает значение урона на основе вероятности попадания в различные части тела
    float DetectionDemage()
    {
        int range = Random.Range(1, 10);
        if (range <= 1)
        {
            // Если попадание в голову, возвращаем высокий уровень урона (50)
            return 50f; 
        }

        else if (range <= 4)
        {
            // Если попадание в тело, возвращаем средний уровень урона (25)
            return 25f; 
        }
        else { return 10f; }   // Если попадание в руку/ногу, возвращаем низкий уровень урона (10)
    }

}
