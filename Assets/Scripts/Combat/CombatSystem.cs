using UnityEngine;
using System.Collections.Generic;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private int maxEnergyPerTurn = 3;
    [SerializeField] private int handSize = 5;

    private List<PlayerCombatState> players = new List<PlayerCombatState>();
    private List<EnemyInstance> enemies = new List<EnemyInstance>();
    private int currentPlayerIndex = 0;
    private int currentEnergy = 0;

    public delegate void CombatEventHandler();
    public static event CombatEventHandler OnTurnStart;
    public static event CombatEventHandler OnTurnEnd;
    public static event CombatEventHandler OnCombatEnd;

    /// <summary>
    /// Initialize combat with players and enemies
    /// </summary>
    public void StartCombat()
    {
        // TODO: Implement combat initialization
        // - Spawn players
        // - Spawn enemies (scaled by player count)
        // - Draw initial hands
        // - Start first player's turn

        Debug.Log("Combat started");
        GameState.Instance.CurrentPhase = GamePhase.Combat_PlayerTurn;
        OnTurnStart?.Invoke();
    }

    /// <summary>
    /// Player plays a card
    /// </summary>
    public void PlayCard(CardInstance card, PlayerCombatState player, EnemyInstance targetEnemy)
    {
        if (currentEnergy < card.Data.cost)
        {
            Debug.LogWarning("Not enough energy to play card");
            return;
        }

        // Apply card effects
        ApplyCardEffects(card.Data, player, targetEnemy);

        currentEnergy -= card.Data.cost;
        Debug.Log($"Played card: {card.Data.cardName}. Energy remaining: {currentEnergy}");
    }

    /// <summary>
    /// Apply card effects to target
    /// </summary>
    private void ApplyCardEffects(CardData card, PlayerCombatState player, EnemyInstance target)
    {
        if (card.damageDealt > 0)
        {
            target?.TakeDamage(card.damageDealt);
            Debug.Log($"Dealt {card.damageDealt} damage");
        }

        if (card.hpRestored > 0)
        {
            player.RestoreHP(card.hpRestored);
            Debug.Log($"Restored {card.hpRestored} HP");
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
        OnTurnEnd?.Invoke();

        // Move to next player or enemy turn
        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Count)
        {
            // All players have gone, enemy turn
            ExecuteEnemyTurns();
            currentPlayerIndex = 0;
        }

        // Reset energy
        currentEnergy = maxEnergyPerTurn;
        GameState.Instance.CurrentPhase = GamePhase.Combat_PlayerTurn;
        OnTurnStart?.Invoke();
    }

    private void ExecuteEnemyTurns()
    {
        GameState.Instance.CurrentPhase = GamePhase.Combat_EnemyTurn;
        Debug.Log("Enemy turn");
        // TODO: Enemy AI turns
    }

    /// <summary>
    /// Check if combat is over
    /// </summary>
    private bool CheckCombatEnd()
    {
        bool allPlayersDefeated = players.TrueForAll(p => !p.IsAlive);
        bool allEnemiesDefeated = enemies.TrueForAll(e => !e.IsAlive);

        if (allPlayersDefeated || allEnemiesDefeated)
        {
            OnCombatEnd?.Invoke();
            GameState.Instance.EndRun(!allPlayersDefeated);
            return true;
        }

        return false;
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
