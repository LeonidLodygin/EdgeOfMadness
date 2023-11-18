using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    float health = 100f;  //изначальное здоровье игрока 
    public TextMeshProUGUI playerHealthText; //UI элемент слева внизу 
    public GameObject bloodOverlay;  // кровь, которая появляется при попадании 
    
    //принимает величину урона (amount) и отнимает ее от уровня здоровья игрока
    public void TakeDamage (float amount)
    {
        //уменьшение текущего здоровья
        health -= amount;

        // Запускаем корутину ShowBlood для отображения эффекта крови при получении урона
        StartCoroutine(ShowBlood()); 
        
        // Если здоровье меньше нуля, то умираем) 
        if (health <= 0)
        {
            Die();
        }
       
    }

    //отображение эффекта крови на экране
    public IEnumerator ShowBlood()
    {
        //Активируем объект bloodOverlay, который представляет из себя наложение крови на экран
        bloodOverlay.SetActive(true);
        
        // Ждем полсекунды, чтобы эффект был виден на экране
        yield return new WaitForSeconds(.5f);
        
        //Деактивируем объект bloodOverlay
        bloodOverlay.SetActive(false);
    }

    private void Update()
    {
        playerHealthText.text = health.ToString(); //обновление уровня здоровья при попадании
    }
    
    //метод смерти - когда уровень здоровья игрока <=0
    void Die()
    {
        SceneManager.LoadScene("SampleScene");  //перезагружаем нашу сцену 
    }
}
