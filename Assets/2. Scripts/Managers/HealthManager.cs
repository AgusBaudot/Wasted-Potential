using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public event Action<int> OnHealthChanged;
    public event Action OnDeath;
    
    [SerializeField] private int maxHealth = 100;
    
    private int _currentHealth;

    private void Awake()
    {
        ServiceLocator.Register(this);
        _currentHealth = maxHealth;
    }

    public void ApplyDamage(int amount)
    {
        _currentHealth -= amount;
        OnHealthChanged?.Invoke(_currentHealth);
        if (_currentHealth <= 0)
            OnDeath?.Invoke();
    }

    private void Die()
    {
        //Do game over or something.
    }
}