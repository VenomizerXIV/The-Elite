using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public HealthBar healthBar;
    public bool isDied = false;
    private Animator animator;
    public event Action<float, float> OnHealthChanged;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trap")
        {
            TakeDamage(100);
        }
    }
    void Die()
    {
        
        if (!isDied)
        {
            rb.bodyType = RigidbodyType2D.Static;
            animator.SetTrigger("IsDied");
            isDied = true;
                      
        }
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
