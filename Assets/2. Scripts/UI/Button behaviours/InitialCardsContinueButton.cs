using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class InitialCardsContinueButton : MonoBehaviour
{
    private ICardManager _cardManager;

    [Inject]
    public void Construct(ICardManager cardManager)
    {
        _cardManager = cardManager ?? throw new ArgumentNullException(nameof(cardManager));
    }
    
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(HandleOnClick);
    }

    private void HandleOnClick()
    {
        _cardManager.CardVisualizer.ConfirmInitialCards();
    }
}