using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This represents the player's "mazo" or library of available blueprints.
/// Created by CardManager
/// </summary>
public class PlayerHand
{
    public List<CardData> AvailableCards { get; } = new List<CardData>();
    public event Action<CardData> OnCardAdded;

    public PlayerHand() { }

    public void AddCard(CardData card)
    {
        AvailableCards.Add(card);
        OnCardAdded?.Invoke(card);
    }
}