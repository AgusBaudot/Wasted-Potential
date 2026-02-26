using System;

public interface IResourcesQuery
{
    event Action<int> OnResourcesChanged;
    
    int CurrentResources { get; }

    bool CanAfford(int amount);
    bool TrySpend(int amount);
    void GainResources(int amount);
}