using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Manages the combat test scene setup
/// </summary>
public class CombatTestSceneManager : MonoBehaviour
{
    [SerializeField] private CombatSystem combatSystem;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private EnemySystem enemySystem;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private CardData.CharacterClass playerClass = CardData.CharacterClass.Fighter;
    [SerializeField] private int playerStartingHP = 50;
    [SerializeField] private int enemyEncounterLevel = 1;

    private void Start()
    {
        InitializeCombat();
    }

    private void InitializeCombat()
    {
        // Setup player
        PlayerCombatState playerState = new PlayerCombatState
        {
            PlayerName = $"{playerClass} Player",
            CurrentHP = playerStartingHP,
            MaxHP = playerStartingHP,
            CurrentArmor = 0
        };

        // Build player deck from class-specific cards
        List<CardData> classCards = CardSystem.Instance.GetCardsByClass(playerClass);
        Debug.Log($"Found {classCards.Count} cards for {playerClass}");

        // Add each class card twice to the deck
        foreach (CardData cardData in classCards)
        {
            playerState.Deck.Add(new CardInstance(cardData, Random.Range(0, 10000)));
            playerState.Deck.Add(new CardInstance(cardData, Random.Range(0, 10000)));
        }

        // Shuffle deck
        for (int i = playerState.Deck.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            CardInstance temp = playerState.Deck[i];
            playerState.Deck[i] = playerState.Deck[randomIndex];
            playerState.Deck[randomIndex] = temp;
        }

        // Setup enemies
        List<EnemyInstance> enemies = enemySystem.GenerateEncounter(enemyEncounterLevel);
        Debug.Log($"Generated {enemies.Count} enemies");

        // Start combat
        combatSystem.StartCombat(playerState, enemies);

        // Setup end turn button
        if (endTurnButton != null)
            endTurnButton.onClick.AddListener(() => combatSystem.EndTurn());
    }
}
