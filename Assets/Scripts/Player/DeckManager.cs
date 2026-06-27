using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private List<CardInstance> currentDeck = new List<CardInstance>();
    [SerializeField] private List<CardInstance> cardsOffered = new List<CardInstance>();

    private int cardInstanceCounter = 0;

    /// <summary>
    /// Add a card to the deck
    /// </summary>
    public void AddCard(CardData cardData)
    {
        CardInstance newCard = new CardInstance(cardData, cardInstanceCounter++);
        currentDeck.Add(newCard);
        Debug.Log($"Added card: {cardData.cardName}. Deck size: {currentDeck.Count}");
    }

    /// <summary>
    /// Remove a card from the deck
    /// </summary>
    public void RemoveCard(CardInstance card)
    {
        currentDeck.Remove(card);
    }

    /// <summary>
    /// Get current deck
    /// </summary>
    public List<CardInstance> GetDeck()
    {
        return new List<CardInstance>(currentDeck);
    }

    /// <summary>
    /// Get random card options to add (typically 3 cards offered per reward)
    /// </summary>
    public List<CardInstance> GetCardOptions(int count = 3)
    {
        cardsOffered.Clear();
        for (int i = 0; i < count; i++)
        {
            CardData randomCard = CardSystem.Instance.GetRandomCard();
            if (randomCard != null)
            {
                cardsOffered.Add(new CardInstance(randomCard, cardInstanceCounter++));
            }
        }
        return cardsOffered;
    }

    public void ClearDeck()
    {
        currentDeck.Clear();
        cardsOffered.Clear();
        cardInstanceCounter = 0;
    }
}
