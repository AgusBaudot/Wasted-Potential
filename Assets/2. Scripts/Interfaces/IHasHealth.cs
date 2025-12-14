using System;

public interface IHasHealth
{
    float Current { get; }
    int Max { get; }
    event Action<float, int> OnHealthChanged;
    event Action OnDeath;
}