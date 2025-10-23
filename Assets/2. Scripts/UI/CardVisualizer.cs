using System;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

/// <summary>
/// Card instantiator.
/// Created by CardManager
/// </summary>
public class CardVisualizer : IDisposable
{
    private RectTransform _cardsContainer;
    private GameObject _cardPrefab;
    private float _horizontalSpacing;
    private List<GameObject> _cards = new List<GameObject>();
    private PlayerHand _playerHand;

    public CardVisualizer(RectTransform cardsContainer, float horizontalSpacing, GameObject cardPrefab, PlayerHand playerHand)
    {
        _cardsContainer = cardsContainer;
        _horizontalSpacing = horizontalSpacing;
        _cardPrefab = cardPrefab;
        _playerHand = playerHand;
        
        playerHand.OnCardAdded += HandleCardAdded;
    }

    private void HandleCardAdded(CardData card)
    {
        //First, Instantiate card as UI.
        GameObject go = GameObject.Instantiate(_cardPrefab, _cardsContainer);

        //Initialize card.
        go.GetComponent<CardDisplay>().Init(card);
        _cards.Add(go);
        
        //Separate them with horizontal spacing.
    }

    public void Dispose()
    {
        _playerHand.OnCardAdded -= HandleCardAdded;
        foreach (var c in _cards)
            Object.Destroy(c);
        _cards.Clear();
    }
}
