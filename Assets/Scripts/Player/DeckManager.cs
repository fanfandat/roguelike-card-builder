using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages player deck operations: adding/removing cards, drawing, and reward selection.
/// Does NOT manage card instances during combat - CombatSystem handles that.
/// </summary>
public class DeckManager : MonoBehaviour
{
    // Runtime lists - not serialized (CardInstance is not serializable)
    private List<CardInstance> currentDeck = new List<CardInstance>();
    private List<CardInstance> cardsOffered = new List<CardInstance>();
    private int cardInstanceCounter = 0;

    public static DeckManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// Add a card to the persistent deck
    /// </summary>
    public void AddCard(CardData cardData)
    {
        CardInstance newCard = new CardInstance(cardData, cardInstanceCounter++);
        currentDeck.Add(newCard);
        Debug.Log($"Added card: {cardData.cardName}. Deck size: {currentDeck.Count}");
    }

    /// <summary>
    /// Remove a card from the persistent deck
    /// </summary>
    public void RemoveCard(CardInstance card)
    {
        currentDeck.Remove(card);
        Debug.Log($"Removed card. Deck size: {currentDeck.Count}");
    }

    /// <summary>
    /// Get a copy of the current deck
    /// </summary>
    public List<CardInstance> GetDeck()
    {
        return new List<CardInstance>(currentDeck);
    }

    /// <summary>
    /// Get random card reward options for a specific class
    /// </summary>
    public List<CardInstance> GetCardOptions(CardData.CharacterClass characterClass, int count = 3)
    {
        cardsOffered.Clear();
        
        for (int i = 0; i < count; i++)
        {
            CardData randomCard = CardSystem.Instance.GetRandomCard(characterClass);
            if (randomCard != null)
            {
                cardsOffered.Add(new CardInstance(randomCard, cardInstanceCounter++));
            }
        }

        Debug.Log($"Generated {cardsOffered.Count} card options for {characterClass}");
        return cardsOffered;
    }

    /// <summary>
    /// Clear entire deck (for run reset)
    /// </summary>
    public void ClearDeck()
    {
        currentDeck.Clear();
        cardsOffered.Clear();
        cardInstanceCounter = 0;
        Debug.Log("Deck cleared");
    }
}
