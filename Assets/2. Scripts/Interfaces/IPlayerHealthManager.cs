using System;

public interface IPlayerHealthManager
{
    event Action<int> OnHealthChanged;
    event Action OnDeath;
    
    void ApplyDamage(int amount);
}