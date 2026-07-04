using UnityEngine;

/// <summary>
/// Core game manager that coordinates global systems and phase transitions.
/// Only handles game state changes and pause logic.
/// Scene-specific initialization is handled by scene managers (e.g., CombatTestSceneManager).
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Subscribe to game state changes
        GameState.OnPhaseChanged += HandlePhaseChange;
    }

    private void OnDestroy()
    {
        GameState.OnPhaseChanged -= HandlePhaseChange;
    }

    /// <summary>
    /// Handle game phase transitions
    /// </summary>
    private void HandlePhaseChange(GamePhase newPhase)
    {
        Debug.Log($"Game phase changed to: {newPhase}");

        switch (newPhase)
        {
            case GamePhase.MainMenu:
                // Load main menu scene
                break;
            case GamePhase.CombatPrep:
                // Prepare for combat (load combat scene)
                break;
            case GamePhase.Combat:
            case GamePhase.Combat_PlayerTurn:
            case GamePhase.Combat_EnemyTurn:
                // Combat phases - handled by CombatSystem
                break;
            case GamePhase.CombatReward:
                // Show card reward screen
                break;
            case GamePhase.BaseHub:
                // Load base building scene
                break;
            case GamePhase.RunComplete:
                // Run completed - show summary
                break;
            case GamePhase.GameOver:
                // Game over screen
                break;
        }
    }

    /// <summary>
    /// Pause or resume the game
    /// </summary>
    public void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
    }
}
