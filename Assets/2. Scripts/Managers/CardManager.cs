using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the central orchestrator. It will manage the pools and the "choose 1 of 3" mechanic. CardManager will observe the WaveManager.
/// </summary>
public class CardManager : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private PlayerHand playerHand;
    
    [Header("Card Pools")]
    [SerializeField] private List<CardData> initialPool;
    [SerializeField] private List<CardData> globalPool;

    private int _wavesUntilChoice = 3;
    private int _waveCounter = 0;

    private void Start()
    {
        waveManager.OnWaveCompleted += HandleWaveCompleted;
        GiveInitialCards();
    }

    private void OnDestroy()
    {
        waveManager.OnWaveCompleted -= HandleWaveCompleted;
    }

    private void GiveInitialCards()
    {
        //For now, gives 3 random cards from the initial pool.
        for (int i = 0; i < 3; i++)
            playerHand.AddCard(initialPool[Random.Range(0, initialPool.Count)]);
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
        playerHand.AddCard(chosenCard);
    }
}
