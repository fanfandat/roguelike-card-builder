using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CombatSystem combatSystem;
    [SerializeField] private UIManager uiManager;

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
        // Subscribe to game state changes
        GameState.OnPhaseChanged += HandlePhaseChange;
    }

    private void OnDestroy()
    {
        GameState.OnPhaseChanged -= HandlePhaseChange;
    }

    private void HandlePhaseChange(GamePhase newPhase)
    {
        Debug.Log($"Game phase changed to: {newPhase}");

        switch (newPhase)
        {
            case GamePhase.MainMenu:
                // Load main menu
                break;
            case GamePhase.CombatPrep:
                // Prepare combat scene
                break;
            case GamePhase.Combat:
                // Start combat
                if (combatSystem != null)
                    combatSystem.StartCombat();
                break;
            case GamePhase.Combat_PlayerTurn:
                // Player's turn
                break;
            case GamePhase.Combat_EnemyTurn:
                // Enemy's turn
                break;
            case GamePhase.CombatReward:
                // Show rewards
                break;
            case GamePhase.BaseHub:
                // Load base building scene
                break;
            case GamePhase.RunComplete:
                // Run completed
                break;
            case GamePhase.GameOver:
                // Game over screen
                break;
        }
    }

    public void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
    }
}
