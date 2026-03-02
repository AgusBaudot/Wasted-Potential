using System;
using UnityEngine;

/// <summary>
/// Simple manager to track the player's money. Uses ServiceLocator for easy access from anywhere.
/// </summary>

public class ResourceManager : MonoBehaviour, IResourcesQuery
{
    public event Action<int> OnResourcesChanged;

    [SerializeField] private int startingResources = 100;
    public int CurrentResources { get; private set; }
    
    private void Awake()
    {
        CurrentResources = startingResources;
    }
    
    public bool CanAfford(int amount) => CurrentResources >= amount;

    public bool TrySpend(int amount)
    {
        if (CanAfford(amount))
        {
            CurrentResources -= amount;
            OnResourcesChanged?.Invoke(CurrentResources);
            return true;
        }

        return false;
    }

    public void GainResources(int amount)
    {
        CurrentResources += amount;
        OnResourcesChanged?.Invoke(CurrentResources);
    }
}