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
    public event Action<CardData> OnCardSelected;
    
    private RectTransform _cardsContainer;
    private GameObject _cardPrefab;
    private List<GameObject> _cards = new List<GameObject>();
    private PlayerHand _playerHand;
    private float _cardSpacing = 130;

    public CardVisualizer(RectTransform cardsContainer, GameObject cardPrefab, PlayerHand playerHand)
    {
        _cardsContainer = cardsContainer;
        _cardPrefab = cardPrefab;
        _playerHand = playerHand;
        
        playerHand.OnCardAdded += HandleCardAdded;
    }

    private void HandleCardAdded(CardData card)
    {
        //First, Instantiate card as UI.
        GameObject go = GameObject.Instantiate(_cardPrefab, _cardsContainer, false);

        //Initialize card.
        go.TryGetComponent<CardDisplay>(out var display);
        display.Init(card);
        display.OnClicked += OnCardDisplayClicked;

        _cards.Add(go);
        
        //Separate them with horizontal spacing.
        for(int i = 0; i < _cards.Count; i++)
        {
            _cards[i].transform.localPosition = new Vector2(_cardSpacing * (i - (_cards.Count - 1) / 2f), 0);
        }
    }

    private void OnCardDisplayClicked(CardData card)
    {
        OnCardSelected?.Invoke(card);
    }

    public void Dispose()
    {
        _playerHand.OnCardAdded -= HandleCardAdded;
        foreach (var c in _cards)
            Object.Destroy(c);
        _cards.Clear();
    }
}
