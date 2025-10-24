using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This is the central orchestrator. It will manage the pools and the "choose 1 of 3" mechanic. CardManager will observe the WaveManager.
/// </summary>
public class CardManager : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    
    [Header("Card Pools")]
    [SerializeField] private List<CardData> initialPool;
    [SerializeField] private List<CardData> globalPool;
    
    [Header("Cards UI")]
    [SerializeField] private RectTransform cardsContainer;
    [SerializeField] private GameObject cardPrefab;

    private int _wavesUntilChoice = 3;
    private int _waveCounter = 0;
    private PlayerHand _playerHand;
    private CardVisualizer _cardVisualizer;
    
    public PlayerHand PlayerHand => _playerHand;
    public CardVisualizer CardVisualizer => _cardVisualizer;

    private void Awake()
    {
        _playerHand = new PlayerHand();
        _cardVisualizer = new CardVisualizer(cardsContainer, cardPrefab, _playerHand);
    }

    private void Start()
    {
        waveManager.OnWaveCompleted += HandleWaveCompleted;
        GiveInitialCards();
    }

    private void OnDestroy()
    {
        waveManager.OnWaveCompleted -= HandleWaveCompleted;
        _cardVisualizer?.Dispose();
    }

    private void GiveInitialCards()
    {
        //For now, gives 3 random cards from the initial pool.
        for (int i = 0; i < 3; i++)
            _playerHand.AddCard(initialPool[Random.Range(0, initialPool.Count)]);
    }

    private void HandleWaveCompleted(int waveIndex)
    {
        _waveCounter++;
        if (_waveCounter >= _wavesUntilChoice)
        {
            _waveCounter = 0;
            PresentCardChoice();
        }
    }

    private void PresentCardChoice()
    {
        //This is where we trigger the UI to show 3 random cards form the globalPool. The UI would then call back with the chosen card.
        Debug.Log("Present Card Choice");
        
        //For now, we simulate the player choosing one randomly.
        CardData chosenCard = globalPool[Random.Range(0, globalPool.Count)];
        _playerHand.AddCard(chosenCard);
    }
}
