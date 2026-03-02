using System.Collections.Generic;

public interface ICardManager
{
    PlayerHand PlayerHand { get; }
    CardVisualizer CardVisualizer { get; }

    void GiveInitialCards();
    void FinalizeInitialCards(List<CardData> cardsToAdd);
    void FinalizeCardChoice(CardData selectedCard);
}