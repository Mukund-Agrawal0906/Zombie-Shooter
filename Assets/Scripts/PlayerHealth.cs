using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("UI Elements")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private float damageFlashDuration = 0.2f;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        UpdateHealthUI();

        // Optional: Add screen flash effect when taking damage
        StartCoroutine(DamageFlashEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }

    private System.Collections.IEnumerator DamageFlashEffect()
    {
        // You can add a UI image flash effect here
        yield return new WaitForSeconds(damageFlashDuration);
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player died!");

        // You can add death effects here

        // Optional: Restart level after short delay
        Invoke("RestartLevel", 2f);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthUI();
    }
}