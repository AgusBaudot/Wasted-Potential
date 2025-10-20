using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This represents the player's "mazo" or library of available blueprints.
/// </summary>
public class PlayerHand : MonoBehaviour
{
    public List<CardData> AvailableCards { get; private set; } = new List<CardData>();

    public void AddCard(CardData card)
    {
        AvailableCards.Add(card);
        Debug.Log($"Added {card.cardName} to hand.");
        //Add event here to update hand UI.
    }
}