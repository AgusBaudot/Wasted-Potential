using System;

public class Health : IHasHealth
{
    public int Max { get; }
    public float Current { get; private set; }

    public event Action<float, int> OnHealthChanged; //Current, max
    public event Action OnDeath;

    public bool IsAlive => Current > 0;

    public Health(int max)
    {
        Max = Math.Max(1, max);
        Current = Max;
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0 || Current <= 0) return;
        Current = Math.Max(0, Current - amount);
        OnHealthChanged?.Invoke(Current, Max);
        if (Current == 0) OnDeath?.Invoke();
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || Current <= 0) return;
        Current = Math.Min(Max, Current + amount);
        OnHealthChanged?.Invoke(Current, Max);
    }
    
    public void Reset() => Current = Max;
}
