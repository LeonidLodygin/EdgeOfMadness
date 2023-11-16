using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    float health = 100f;
    public TextMeshProUGUI playerHealthText;
    public GameObject bloodOverlay; 
    public void TakeDamage (float amount)
    {

        health -= amount;
        StartCoroutine(ShowBlood()); // Запуск корутины без не работало
        if (health <= 0)
        {
            Die();
        }
       
    }
    public IEnumerator ShowBlood()
    {
        bloodOverlay.SetActive(true);
        yield return new WaitForSeconds(.5f);
        bloodOverlay.SetActive(false);
    }

    private void Update()
    {
        playerHealthText.text = health.ToString();
    }
    void Die()
    {
        SceneManager.LoadScene("SampleScene"); 
    }
}
