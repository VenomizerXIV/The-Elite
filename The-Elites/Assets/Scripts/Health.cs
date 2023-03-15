using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public HealthBar healthBar;

    public bool isDied = false;
    private Animator animator;
    public event Action<float, float> OnHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (healthBar == null)
        {
            Debug.LogError("HealthBar is not assigned in the Inspector!");
        }
        else
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        Debug.Log("It hurts");
        if (currentHealth <= 0)
        {
            Die();
        }

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Die()
    {
        // Handle object destruction, animation, sound effects, etc.
        // Destroy(gameObject);
        if (!isDied)
        {
            animator.SetTrigger("IsDied");
            isDied = true;
        }
    }
}
