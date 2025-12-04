using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Action<int, int> OnHealthChanged;
    public Action OnDied;
    
    [SerializeField] protected int maxHealth = 100;

    private int currentHealth;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    protected virtual void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    protected virtual void Die()
    {
        OnDied?.Invoke();
        
        Destroy(gameObject);
    }
    
    public void RefreshState()
    {
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }
}