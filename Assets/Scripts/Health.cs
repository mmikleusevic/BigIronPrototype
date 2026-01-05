using System;
using CombatRoom;
using Managers;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Action<int, int> OnHealthChanged;
    
    [SerializeField] private AudioClip healClip;
    [SerializeField] private AudioClip hurtClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] protected int maxHealth = 100;

    private int currentHealth;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    protected virtual void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public virtual int TakeDamage(Combatant damager, Combatant receiver, int healthToLose)
    {
        int actualLoss = Mathf.Min(CurrentHealth, healthToLose);
        
        currentHealth -= actualLoss;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (actualLoss > 0 && currentHealth > 0) SoundManager.Instance.PlayVFX(hurtClip);
        
        if (currentHealth <= 0) Die(damager, receiver);
        
        return actualLoss;
    }

    public int Heal(int healAmount)
    {
        int missingHealth = maxHealth - currentHealth;
        int actualHeal = Mathf.Clamp(healAmount, 0, missingHealth);

        if (actualHeal > 0) SoundManager.Instance.PlayVFX(healClip);
        
        currentHealth += actualHeal;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        return actualHeal;
    }

    protected virtual void Die(Combatant damager, Combatant receiver)
    {
        SoundManager.Instance.PlayVFX(deathClip);
    }
    
    public void RefreshState()
    {
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }
}