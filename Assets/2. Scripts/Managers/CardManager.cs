using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This is the central orchestrator. It will manage the pools and the "choose 1 of 3" mechanic. CardManager will observe the WaveManager.
/// </summary>
public class CardManager : MonoBehaviour
{
    
    [Header("Card Pools")]
    [SerializeField] private List<CardData> initialPool;
    [SerializeField] private List<CardData> globalPool;
    
    [Header("Cards UI")]
    [SerializeField] private RectTransform cardsContainer;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject initialCardPrefab;
    [SerializeField] private GameObject choiceCardPrefab;

    [Header("UI Containers")]
    [SerializeField] private RectTransform initialCardsPanel;
    [SerializeField] private RectTransform cardChoicePanel;
    [SerializeField] private GameObject playingUI;
    
    private PlayerHand _playerHand;
    private CardVisualizer _cardVisualizer;
    private WaveManager waveManager;
    
    public PlayerHand PlayerHand => _playerHand;
    public CardVisualizer CardVisualizer => _cardVisualizer;

    #region Base weights for card pool rarity

    private const float WEIGHT_COMMON = 70;
    private const float WEIGHT_RARE = 25;
    private const float WEIGHT_EPIC = 5;

    #endregion

    private void Awake()
    {
        _playerHand = new PlayerHand();
        _cardVisualizer = new CardVisualizer(cardsContainer, initialCardsPanel, cardChoicePanel, cardPrefab, initialCardPrefab, choiceCardPrefab, _playerHand);
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        waveManager = ServiceLocator.Get<WaveManager>();
        waveManager.OnNewCardOffer += HandleCardOffered;
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
        waveManager.OnNewCardOffer -= HandleCardOffered;
        _cardVisualizer?.Dispose();
    }

    public void GiveInitialCards()
    {
        var initial3 = new List<CardData>();
        for (int i = 0; i < 3; i++)
        {
            var card = initialPool[Random.Range(0, initialPool.Count)];
            initial3.Add(card);
            initialPool.Remove(card);
            if (globalPool.Contains(card))
                globalPool.Remove(card);
        }
        
        _cardVisualizer.ShowInitialCards(initial3);
    }

    public void FinalizeInitialCards(List<CardData> cardsToAdd)
    {
        foreach (var c in cardsToAdd)
            _playerHand.AddCard(c);
        
        //Hide/clear initial cards/panel UI.
        initialCardsPanel.transform.parent.gameObject.SetActive(false);
        playingUI.SetActive(true);
    }
    
    public void FinalizeCardChoice(CardData selectedCard)
    {
        _playerHand.AddCard(selectedCard);

        if (globalPool.Contains(selectedCard))
            globalPool.Remove(selectedCard);
        
        cardChoicePanel.transform.parent.gameObject.SetActive(false);
        playingUI.SetActive(true);
    }

    private void HandleCardOffered()
    {
        PresentCardChoice();
    }
    
    private void PresentCardChoice()
    {
        cardChoicePanel.transform.parent.gameObject.SetActive(true);

        var selection = new List<CardData>();
        
        // 1. Create a temporary list that copies the globalPool.
        // We modify this list instead of the master list.
        List<CardData> tempPool = new List<CardData>(globalPool);

        for (int i = 0; i < 3; i++)
        {
            // 2. Pass the tempPool to your weight calculator
            CardData card = GetWeightedCard(tempPool);
            
            if (card != null) 
            {
                selection.Add(card);
                // 3. Remove from tempPool so we don't get duplicates 
                // in the SAME choice of 3 cards.
                tempPool.Remove(card); 
            }
        }
        
        _cardVisualizer.ShowCardChoice(selection);
    }

    #region Card choice selection
    
    private CardData GetWeightedCard(List<CardData> availableCards)
    {
        //1. Separate available cards into buckets.
        List<CardData> commons = availableCards.Where(c => c.rarity == CardRarity.Common).ToList();
        List<CardData> rares = availableCards.Where(c => c.rarity == CardRarity.Rare).ToList();
        List<CardData> epics =  availableCards.Where(c => c.rarity == CardRarity.Epic).ToList();
        
        //2. Calculate valid weights (if a list is empty, its weight is 0).
        float currentCommonWeight = commons.Count > 0 ? WEIGHT_COMMON : 0;
        float currentRareWeight = rares.Count > 0 ? WEIGHT_RARE : 0;
        float currentEpicWeight = epics.Count > 0 ? WEIGHT_EPIC : 0;
        
        //3. Sum total weight to create "die size"
        float totalWeight = currentCommonWeight + currentRareWeight + currentEpicWeight;
        
        //Safety check: pool is empty
        if (totalWeight <= 0)
        {
            Debug.LogWarning("Card pool is empty!");
            return null;
        }
        
        //4. Roll the dice
        float randomValue = Random.Range(0, totalWeight);
        
        //5. Determine which bucket we landed in checking "ranges" of probability
        if (randomValue < currentCommonWeight)
            return GetRandomFromList(commons);
        else if (randomValue < currentCommonWeight + currentRareWeight)
            return GetRandomFromList(rares);
        else
            return GetRandomFromList(epics);
    }

    private CardData GetRandomFromList(List<CardData> list)
    {
        if (list.Count == 0) return null; //Should not happen given logic above
        return list[Random.Range(0, list.Count)];
    }
    #endregion
}
