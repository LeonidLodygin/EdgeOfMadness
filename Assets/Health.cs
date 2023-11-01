using NUnit.Framework;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    float health = 100f;
    public TextMeshProUGUI playerHealthText;

    public void TakeDamage (float amount)
    {
        health -= amount;
        if (health < 0)
        {
            Die();
        }
    }
    private void Update()
    {
        playerHealthText.text = health.ToString();
    }
    void Die()
    {
        //Destroy(gameObject);
    }
}
