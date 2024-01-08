using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    float health = 100f;  // Initial player health 
    public TextMeshProUGUI playerHealthText; // UI element at the bottom left
    public GameObject bloodOverlay;  // Blood overlay that appears when hit 

    // Takes the damage amount (amount) and subtracts it from the player's health level
    public void TakeDamage(float amount)
    {
        // Decrease the current health
        health -= amount;

        // Start the ShowBlood coroutine to display the blood effect when taking damage

        // StartCoroutine(ShowBlood()); 

        // If health is less than or equal to zero, then die) 
        if (health <= 0)
        {
            Die();
        }
    }

    // Display blood effect on the screen
    public IEnumerator ShowBlood()
    {
        // Activate the bloodOverlay object, representing a blood overlay on the screen
        bloodOverlay.SetActive(true);

        // Wait for half a second to make the effect visible on the screen
        yield return new WaitForSeconds(.5f);

        // Deactivate the bloodOverlay object
        bloodOverlay.SetActive(false);
    }

    private void Update()
    {
        playerHealthText.text = health.ToString(); // Update the health level when hit
    }

    // Death method - when the player's health level is <=0
    void Die()
    {
        SceneManager.LoadScene("SampleScene");  // Reload our scene 
    }
}
