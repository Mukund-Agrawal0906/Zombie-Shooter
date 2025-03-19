using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float health;
    private float damage;
    private HealthBar healthBar;
    private Transform player;

    public void Initialize(float startHealth, float enemyDamage, HealthBar bar)
    {
        health = startHealth;
        damage = enemyDamage;
        healthBar = bar;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            transform.LookAt(player);
        }

        if (healthBar != null)
        {
            healthBar.transform.LookAt(Camera.main.transform);
        }
    }


    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.UpdateHealth(health);

        if (health <= 0)
        {
            FindFirstObjectByType<GameManager>().OnEnemyDefeated(gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage * Time.deltaTime);
            }
        }
    }
}
