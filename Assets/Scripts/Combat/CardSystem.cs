using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CardSystem : MonoBehaviour
{
    [SerializeField] private List<CardData> allCards = new List<CardData>();

    public static CardSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        // Load all cards from Resources folder
        LoadCardsFromResources();
    }

    private void LoadCardsFromResources()
    {
        CardData[] loadedCards = Resources.LoadAll<CardData>("Cards");
        allCards.AddRange(loadedCards);
        Debug.Log($"Loaded {allCards.Count} cards");
    }

    /// <summary>
    /// Get a random card from the pool (respecting rarity distribution)
    /// </summary>
    public CardData GetRandomCard(CardData.Rarity maxRarity = CardData.Rarity.Legendary)
    {
        var validCards = allCards.Where(c => (int)c.rarity <= (int)maxRarity).ToList();
        return validCards.Count > 0 ? validCards[Random.Range(0, validCards.Count)] : null;
    }

    /// <summary>
    /// Get cards by type
    /// </summary>
    public List<CardData> GetCardsByType(CardData.CardType type)
    {
        return allCards.Where(c => c.cardType == type).ToList();
    }
}
