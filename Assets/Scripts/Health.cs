using System;
using CombatRoom;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Action<int, int> OnHealthChanged;
    
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

        if (currentHealth <= 0) Die(damager, receiver);
        
        return actualLoss;
    }

    public int Heal(int healAmount)
    {
        int missingHealth = maxHealth - currentHealth;
        int actualHeal = Mathf.Clamp(healAmount, 0, missingHealth);

        currentHealth += actualHeal;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        return actualHeal;
    }

    protected virtual void Die(Combatant damager, Combatant receiver)
    {
        if (!damager && !receiver) return;
        
        damager.GainGoldAmount(receiver.Gold.GoldAmount);
    }
    
    public void RefreshState()
    {
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }
}