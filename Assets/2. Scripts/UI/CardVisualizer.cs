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
    public event Action OnCardDeselected;
    public event Action<List<CardData>> OnInitialConfirmed;

    private List<GameObject> _initialCardGOs = new();
    private List<CardData> _initialCardDatas = new();

    private RectTransform _cardsContainer;
    private RectTransform _initialCardsContainer;
    private RectTransform _choiceCardsContainer;
    
    private GameObject _cardPrefab;
    private GameObject _initialCardPrefab;
    private GameObject _choiceCardPrefab;
    
    private List<GameObject> _cards = new();
    private PlayerHand _playerHand;
    private float _cardSpacing = 200;
    private GameObject _selectedCard;

    public CardVisualizer(RectTransform cardsContainer, RectTransform initialCardsPanel, RectTransform choiceCardsContainer, GameObject cardPrefab, GameObject initialCardPrefab, GameObject choiceCardPrefab, PlayerHand playerHand)
    {
        _cardsContainer = cardsContainer;
        _initialCardsContainer = initialCardsPanel;
        _choiceCardsContainer = choiceCardsContainer;
        
        _cardPrefab = cardPrefab;
        _initialCardPrefab = initialCardPrefab;
        _choiceCardPrefab = choiceCardPrefab;
        
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
        for (int i = 0; i < _cards.Count; i++)
        {
            if (_cards[i].GetComponent<CardDisplay>().Selected) continue;
            _cards[i].transform.localPosition = new Vector2(_cardSpacing * (i - (_cards.Count - 1) / 2f), 0);
        }
    }

    public void ShowInitialCards(List<CardData> initialCards)
    {
        ClearInitialCards();

        _initialCardDatas = new List<CardData>(initialCards);

        for (int i = 0; i < initialCards.Count; i++)
        {
            var go = GameObject.Instantiate(_initialCardPrefab, _initialCardsContainer, false);
            var rt = go.GetComponent<RectTransform>();

            //Center arrangement for exactly 3 cards
            float offset = 0f;
            if (i == 0) offset = -_cardSpacing;   //Left
            else if (i == 2) offset = _cardSpacing; //Right

            rt.anchoredPosition = new Vector2(offset * 3, 0);

            go.TryGetComponent<CardDisplay>(out var display);
            display.IsInteractive = false;
            display.Init(initialCards[i]);

            _initialCardGOs.Add(go);
        }
    }

    public void ShowCardChoice(List<CardData> choice)
    {
        for (int i = 0; i < choice.Count; i++)
        {
            var go = GameObject.Instantiate(_choiceCardPrefab, _choiceCardsContainer, false);
            var rt = go.GetComponent<RectTransform>();
            
            //Center arrangement for 3 cards
            float offset = 0;
            if (i == 0) offset = -_cardSpacing;
            else if (i == 2) offset = _cardSpacing;
            
            rt.anchoredPosition = new Vector2(offset * 3, 0);
            
            go.TryGetComponent<CardDisplay>(out var display);
            display.Init(choice[i]);
            display.IsInteractive = false;
            
            _initialCardGOs.Add(go);
        }
    }

    public void MoveInitialsToHand(float duration = 0.5f)
    {
        Debug.LogWarning("Missing UI movement into bottom left.");
    }

    //Called by UI
    public void ConfirmInitialCards()
    {
        OnInitialConfirmed?.Invoke(new List<CardData>(_initialCardDatas));
    }

    public void ClearInitialCards()
    {
        foreach (var go in _initialCardGOs) Object.Destroy(go);
        _initialCardGOs.Clear();
        _initialCardDatas.Clear();
    }

    private void OnCardDisplayClicked(CardData data, GameObject card)
    {
        OnCardSelected?.Invoke(data);
        MoveSelectedCard(card);
    }

    private void MoveSelectedCard(GameObject card)
    {
        //Case 1: if no other card was selected.
        if (_selectedCard == null)
        {
            _selectedCard = card;
            _selectedCard.GetComponent<CardDisplay>().MoveCard();
        }
        //Case 2: if other card was selected.
        else if (_selectedCard != card)
        {
            _selectedCard.GetComponent<CardDisplay>().MoveCard();
            _selectedCard = card;
            _selectedCard.GetComponent<CardDisplay>().MoveCard();
        }
        //Case 3: if selected card was clicked.
        else if (_selectedCard == card)
        {
            _selectedCard.GetComponent<CardDisplay>().MoveCard();
            _selectedCard = null;
            OnCardDeselected?.Invoke();
        }
    }

    public void DeselectCard()
    {
        _selectedCard?.GetComponent<CardDisplay>().MoveCard();
        _selectedCard = null;
    }

    public void Dispose()
    {
        _playerHand.OnCardAdded -= HandleCardAdded;
        foreach (var c in _cards)
            Object.Destroy(c);
        _cards.Clear();
    }
}