using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHP : MonoBehaviour
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
            animator.SetTrigger("IsDied");
            isDied = true;
                      
        }
    }

}