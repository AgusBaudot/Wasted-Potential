using System;

public interface IHasHealth
{
    int Current { get; }
    int Max { get; }
    event Action<int, int> OnHealthChanged;
    event Action OnDeath;
}