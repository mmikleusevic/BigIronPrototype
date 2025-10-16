using System;
using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    public event Action<int> OnGoldChanged;
    public event Action<int> OnHpChanged;
    public event Action OnPlayerDeath;
    
    [SerializeField] private int MaxHealth;
    public int CurrentHealth { get; private set; }
    public int Gold { get; private set; }

    private void Start()
    {
        CurrentHealth = MaxHealth;
        Gold = 0;
    }
    
    public void ChangeGoldAmount(int amount)
    {
        Gold += amount;
        OnGoldChanged?.Invoke(Gold);
    }

    public void LoseHealth(int amount)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        OnHpChanged?.Invoke(CurrentHealth);

        if (CurrentHealth == 0) OnPlayerDeath?.Invoke();
    }
}
