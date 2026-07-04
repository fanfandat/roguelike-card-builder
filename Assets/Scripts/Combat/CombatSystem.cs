using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private int maxEnergyPerTurn = 3;
    [SerializeField] private int handSize = 5;
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private Transform playerHandContainer;
    [SerializeField] private GameObject cardDisplayPrefab;
    [SerializeField] private Text energyDisplay;
    [SerializeField] private Text playerHPDisplay;
    [SerializeField] private Text enemyHPDisplay;

    private PlayerCombatState playerState;
    private List<EnemyInstance> enemies = new List<EnemyInstance>();
    private int currentEnergy = 0;
    private int currentTurn = 0;

    public delegate void CombatEventHandler();
    public static event CombatEventHandler OnTurnStart;
    public static event CombatEventHandler OnTurnEnd;
    public static event CombatEventHandler OnCombatEnd;

    private void Start()
    {
        // Initialization happens in StartCombat()
    }

    /// <summary>
    /// Initialize combat with a player and enemies
    /// </summary>
    public void StartCombat(PlayerCombatState player, List<EnemyInstance> enemyList)
    {
        playerState = player;
        enemies = new List<EnemyInstance>(enemyList);
        currentEnergy = maxEnergyPerTurn;
        currentTurn = 0;

        Debug.Log($"Combat started! Player HP: {playerState.CurrentHP}, Enemies: {enemies.Count}");

        GameState.Instance.CurrentPhase = GamePhase.Combat_PlayerTurn;
        DrawInitialHand();
        UpdateUI();
        OnTurnStart?.Invoke();
    }

    /// <summary>
    /// Draw initial hand of 5 cards
    /// </summary>
    private void DrawInitialHand()
    {
        for (int i = 0; i < handSize; i++)
        {
            DrawCard();
        }
        RefreshHandDisplay();
    }

    /// <summary>
    /// Draw a single card from deck to hand
    /// </summary>
    private void DrawCard()
    {
        if (playerState.Deck.Count == 0)
        {
            // Reshuffle from discard
            playerState.Deck.AddRange(playerState.Discard);
            playerState.Discard.Clear();
        }

        if (playerState.Deck.Count > 0)
        {
            CardInstance card = playerState.Deck[0];
            playerState.Deck.RemoveAt(0);
            playerState.Hand.Add(card);
        }
    }

    /// <summary>
    /// Refresh the visual display of cards in hand
    /// </summary>
    private void RefreshHandDisplay()
    {
        // Clear old cards
        foreach (Transform child in playerHandContainer)
        {
            Destroy(child.gameObject);
        }

        // Display new cards
        foreach (CardInstance card in playerState.Hand)
        {
            GameObject cardDisplay = Instantiate(cardDisplayPrefab, playerHandContainer);
            CardDisplayButton cardButton = cardDisplay.GetComponent<CardDisplayButton>();
            if (cardButton != null)
            {
                cardButton.Initialize(card, this);
            }
        }
    }

    /// <summary>
    /// Player plays a card
    /// </summary>
    public void PlayCard(CardInstance card)
    {
        if (currentEnergy < card.Data.cost)
        {
            Debug.LogWarning("Not enough energy to play card");
            return;
        }

        if (!playerState.Hand.Contains(card))
        {
            Debug.LogWarning("Card not in hand");
            return;
        }

        // Get first enemy as target (can be expanded for targeting)
        if (enemies.Count == 0)
        {
            Debug.LogWarning("No enemies to target");
            return;
        }

        EnemyInstance targetEnemy = enemies[0];

        // Apply card effects
        ApplyCardEffects(card.Data, playerState, targetEnemy);

        // Spend energy and discard card
        currentEnergy -= card.Data.cost;
        playerState.Hand.Remove(card);
        playerState.Discard.Add(card);

        Debug.Log($"Played card: {card.Data.cardName}. Energy: {currentEnergy}/{maxEnergyPerTurn}");

        UpdateUI();
        RefreshHandDisplay();

        // Check if combat is over
        if (CheckCombatEnd())
            return;
    }

    /// <summary>
    /// Apply card effects to player/enemy
    /// </summary>
    private void ApplyCardEffects(CardData card, PlayerCombatState player, EnemyInstance target)
    {
        if (card.damageDealt > 0)
        {
            target.TakeDamage(card.damageDealt);
            Debug.Log($"Dealt {card.damageDealt} damage to {target.Data.enemyName}. Enemy HP: {target.CurrentHP}");
        }

        if (card.hpRestored > 0)
        {
            player.RestoreHP(card.hpRestored);
            Debug.Log($"Restored {card.hpRestored} HP to player");
        }

        if (card.armorGained > 0)
        {
            player.GainArmor(card.armorGained);
            Debug.Log($"Gained {card.armorGained} armor");
        }
    }

    /// <summary>
    /// End current player's turn
    /// </summary>
    public void EndTurn()
    {
        Debug.Log("=== End Player Turn ===");
        OnTurnEnd?.Invoke();

        // Enemy turn
        ExecuteEnemyTurns();

        // Next turn
        currentTurn++;
        currentEnergy = maxEnergyPerTurn;
        DrawCard(); // Draw 1 card for next turn
        RefreshHandDisplay();
        UpdateUI();

        GameState.Instance.CurrentPhase = GamePhase.Combat_PlayerTurn;
        Debug.Log("=== Start Player Turn ===");
        OnTurnStart?.Invoke();
    }

    /// <summary>
    /// Execute all enemy turns
    /// </summary>
    private void ExecuteEnemyTurns()
    {
        GameState.Instance.CurrentPhase = GamePhase.Combat_EnemyTurn;
        Debug.Log("=== Enemy Turn ===");

        foreach (EnemyInstance enemy in enemies)
        {
            if (!enemy.IsAlive) continue;

            // Simple AI: Just attack player
            // TODO: Implement more complex AI behaviors
            playerState.TakeDamage(5); // Fixed 5 damage for now
            Debug.Log($"{enemy.Data.enemyName} attacks! Player HP: {playerState.CurrentHP}");
        }
    }

    /// <summary>
    /// Check if combat is over
    /// </summary>
    private bool CheckCombatEnd()
    {
        // Remove dead enemies
        enemies.RemoveAll(e => !e.IsAlive);

        bool allPlayersDefeated = !playerState.IsAlive;
        bool allEnemiesDefeated = enemies.Count == 0;

        if (allPlayersDefeated || allEnemiesDefeated)
        {
            OnCombatEnd?.Invoke();
            bool victory = !allPlayersDefeated;
            GameState.Instance.EndRun(victory);
            
            if (victory)
                Debug.Log("=== VICTORY ===");
            else
                Debug.Log("=== DEFEAT ===");
            
            return true;
        }

        return false;
    }

    /// <summary>
    /// Update UI displays
    /// </summary>
    private void UpdateUI()
    {
        if (energyDisplay != null)
            energyDisplay.text = $"Energy: {currentEnergy}/{maxEnergyPerTurn}";

        if (playerHPDisplay != null)
            playerHPDisplay.text = $"Player HP: {playerState.CurrentHP}/{playerState.MaxHP}";

        if (enemyHPDisplay != null && enemies.Count > 0)
            enemyHPDisplay.text = $"{enemies[0].Data.enemyName} HP: {enemies[0].CurrentHP}";
    }
}

/// <summary>
/// Player's combat state during a battle
/// </summary>
public class PlayerCombatState
{
    public string PlayerName { get; set; }
    public int CurrentHP { get; set; }
    public int MaxHP { get; set; }
    public int CurrentArmor { get; set; }
    public List<CardInstance> Hand { get; set; } = new List<CardInstance>();
    public List<CardInstance> Deck { get; set; } = new List<CardInstance>();
    public List<CardInstance> Discard { get; set; } = new List<CardInstance>();

    public bool IsAlive => CurrentHP > 0;

    public void TakeDamage(int damage)
    {
        int armorReduction = Mathf.Min(CurrentArmor, damage);
        damage -= armorReduction;
        CurrentArmor -= armorReduction;
        CurrentHP -= Mathf.Max(1, damage);
    }

    public void RestoreHP(int amount)
    {
        CurrentHP = Mathf.Min(CurrentHP + amount, MaxHP);
    }

    public void GainArmor(int amount)
    {
        CurrentArmor += amount;
    }
}
