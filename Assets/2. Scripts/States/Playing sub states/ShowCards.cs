using System.Collections.Generic;
using VContainer;

public class ShowCards : ISubState
{
    private ICardManager _cardManager;

    public ShowCards(ICardManager manager)
    {
        _cardManager = manager;
    }

    public void Enter()
    {
        //Show initial cards
        _cardManager.GiveInitialCards();
        _cardManager.CardVisualizer.OnInitialConfirmed += HandleInitialConfirmed;
    }

    public void Update() { }

    public void Exit()
    {
        _cardManager.CardVisualizer.OnInitialConfirmed -= HandleInitialConfirmed;
    }

    private void HandleInitialConfirmed(List<CardData> cards)
    {
        _cardManager.FinalizeInitialCards(cards);

        _cardManager.CardVisualizer.MoveInitialsToHand();
        
        _cardManager.CardVisualizer.OnInitialConfirmed -= HandleInitialConfirmed;
    }
}
