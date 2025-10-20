using System;
using UnityEngine;

/// <summary>
/// Simple manager to track the player's money. Uses ServiceLocator for easy access from anywhere.
/// </summary>
public class ResourceManager : MonoBehaviour
{
    public event Action<int> OnResourcesChanged;
    
    [SerializeField] private int startingResources = 100;
    public int CurrentResources { get; private set; }
    
    private void Awake()
    {
        ServiceLocator.Register(this);
        CurrentResources = startingResources;
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
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

    public void GainResources(int amonut)
    {
        CurrentResources += amonut;
        OnResourcesChanged?.Invoke(CurrentResources);
    }
}