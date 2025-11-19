using System.Collections.Generic;
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

    [Header("UI Containers")]
    [SerializeField] private RectTransform initialCardsPanel;
    [SerializeField] private GameObject playingUI;

    private int _wavesUntilChoice = 3;
    private int _waveCounter = 0;
    private PlayerHand _playerHand;
    private CardVisualizer _cardVisualizer;
    private WaveManager waveManager;
    
    public PlayerHand PlayerHand => _playerHand;
    public CardVisualizer CardVisualizer => _cardVisualizer;

    private void Awake()
    {
        _playerHand = new PlayerHand();
        _cardVisualizer = new CardVisualizer(cardsContainer, initialCardsPanel, cardPrefab, _playerHand);
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        waveManager = ServiceLocator.Get<WaveManager>();
        waveManager.OnWaveCompleted += HandleWaveCompleted;
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
        waveManager.OnWaveCompleted -= HandleWaveCompleted;
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
            //if (globalPool.Contains(card))
            //    globalPool.Remove(card);
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
        Debug.LogWarning("Present Card Choice");

        //For now, we simulate the player choosing one randomly.
        CardData chosenCard = globalPool[Random.Range(0, globalPool.Count)];
        _playerHand.AddCard(chosenCard);
    }
}
