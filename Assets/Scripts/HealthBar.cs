using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    private float maxHealth;

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
    }

    public void UpdateHealth(float currentHealth)
    {
        fillImage.fillAmount = currentHealth / maxHealth;
    }
}