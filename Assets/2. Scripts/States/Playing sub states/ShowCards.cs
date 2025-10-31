using UnityEngine;

public class ShowCards : ISubState
{
    //First, show cards and let player advance on button press.
    //Then, animate cards.
    //After animation completes, add cards to their container.

    private CardManager _cardManager;

    public ShowCards()
    {
        _cardManager = ServiceLocator.Get<CardManager>();
    }

    public void Enter()
    {
        //Show cards centered.
        _cardManager.CardVisualizer.ShowInitialCards();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}
