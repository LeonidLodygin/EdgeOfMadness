using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class Weapon_Shoot : MonoBehaviour
{
    //public float range = 100f; 
    
    int impactForce = 3000; // ���� ��� ����������� �� ������ ��� �������� � ����

    Transform enemy; //���
    Transform player; //�����
    Animator enemyAnimator; //�������� ����


    public VisualEffect muzzleFlash; // ������ ������� ��� ��������
    public Transform firePoint; // �����, ������ "�����������" ��������

    private float nextTimeToFire; //�������� ����� ���������� 


    private void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy1").transform; 
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAnimator = enemy.GetComponent<Animator>();
    }
    
    
    void Update()
    {
        // ����������� ����� �� ���������� ��������
        nextTimeToFire += 1 * Time.deltaTime;

        // ���������, ������ �� ���������� ������� � ����������� �������� � ��� � ��������� �����
        if (nextTimeToFire>1.2)
        {
            nextTimeToFire = 0;  // ���������� ����� �� ���������� ��������
            Shoot(); // �������� ����� ��� ���������� ��������
        }
    }

    // ������� ��� ���������� ��������, ������� ��������������� ������� �������,
    // ����������� ��������� � ���������� �����
    void Shoot()
    {
        // ������������� ������ �������� 
        muzzleFlash.Play();

        // ������� ���, ������� ������������ ����� ����� �� ������� ���� � ����������� ��� ������.
        RaycastHit hit;
        if (Physics.Raycast(enemy.transform.position, enemy.transform.forward, out hit))
        {
            //
            //Debug.Log(hit.transform.name);

            // �������� ��������� Health �������, � ������� ����� ���
            Health target = hit.transform.GetComponent<Health>();

            // ���� � ������� ���� ��������� Health, ��������� ���� (������������, ����, ���� �� ������ � ������)
            if (target != null)
            {
                // ���������� ������� ����� � �������������� ������� DetectionDemage
                float damage = DetectionDemage();
                // �������� ���� ������� Health
                target.TakeDamage(damage);
            }
        };
    }

    // ������� ��� ����������� ������ ����� � ����������� �� ��������� ������� ���������
    // ���������� �������� ����� �� ������ ����������� ��������� � ��������� ����� ����
    float DetectionDemage()
    {
        int range = Random.Range(1, 10);
        if (range <= 1)
        {
            // ���� ��������� � ������, ���������� ������� ������� ����� (50)
            return 50f; 
        }

        else if (range <= 4)
        {
            // ���� ��������� � ����, ���������� ������� ������� ����� (25)
            return 25f; 
        }
        else { return 10f; }   // ���� ��������� � ����/����, ���������� ������ ������� ����� (10)
    }

}
