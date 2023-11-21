using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    float health = 100f;  //����������� �������� ������ 
    public TextMeshProUGUI playerHealthText; //UI ������� ����� ����� 
    public GameObject bloodOverlay;  // �����, ������� ���������� ��� ��������� 
    
    //��������� �������� ����� (amount) � �������� �� �� ������ �������� ������
    public void TakeDamage (float amount)
    {
        //���������� �������� ��������
        health -= amount;

        // ��������� �������� ShowBlood ��� ����������� ������� ����� ��� ��������� �����
        StartCoroutine(ShowBlood()); 
        
        // ���� �������� ������ ����, �� �������) 
        if (health <= 0)
        {
            Die();
        }
       
    }

    //����������� ������� ����� �� ������
    public IEnumerator ShowBlood()
    {
        //���������� ������ bloodOverlay, ������� ������������ �� ���� ��������� ����� �� �����
        bloodOverlay.SetActive(true);
        
        // ���� ����������, ����� ������ ��� ����� �� ������
        yield return new WaitForSeconds(.5f);
        
        //������������ ������ bloodOverlay
        bloodOverlay.SetActive(false);
    }

    private void Update()
    {
        playerHealthText.text = health.ToString(); //���������� ������ �������� ��� ���������
    }
    
    //����� ������ - ����� ������� �������� ������ <=0
    void Die()
    {
        SceneManager.LoadScene("SampleScene");  //������������� ���� ����� 
    }
}
