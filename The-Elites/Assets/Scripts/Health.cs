using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public bool isDied = false;
    private Animator animator;
    public event Action<float, float> OnHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
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
