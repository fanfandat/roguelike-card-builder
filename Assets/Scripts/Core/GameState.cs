using UnityEngine;

public enum GamePhase
{
    MainMenu,
    CombatPrep,
    Combat,
    Combat_PlayerTurn,
    Combat_EnemyTurn,
    CombatReward,
    BaseHub,
    RunComplete,
    GameOver
}

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    [SerializeField] private GamePhase currentPhase = GamePhase.MainMenu;
    public GamePhase CurrentPhase
    {
        get => currentPhase;
        set
        {
            if (currentPhase != value)
            {
                currentPhase = value;
                OnPhaseChanged?.Invoke(currentPhase);
            }
        }
    }

    // Game state data
    public int CurrentRunNumber { get; set; } = 0;
    public int PlayerCount { get; set; } = 1;
    public float DifficultyScalar { get; private set; } = 1.0f;

    // Events
    public delegate void PhaseChangedEvent(GamePhase newPhase);
    public static event PhaseChangedEvent OnPhaseChanged;

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
        // Initialize game state
        CurrentPhase = GamePhase.MainMenu;
    }

    /// <summary>
    /// Calculate difficulty scalar based on player count
    /// More players = harder enemies
    /// </summary>
    public void SetPlayerCount(int count)
    {
        PlayerCount = Mathf.Clamp(count, 1, 4);
        DifficultyScalar = 1.0f + (PlayerCount - 1) * 0.25f; // 1.0x for 1 player, 1.75x for 4 players
        Debug.Log($"Player count set to {PlayerCount}. Difficulty scalar: {DifficultyScalar}");
    }

    public void StartNewRun()
    {
        CurrentRunNumber++;
        CurrentPhase = GamePhase.CombatPrep;
        Debug.Log($"Starting run #{CurrentRunNumber}");
    }

    public void EndRun(bool victory)
    {
        CurrentPhase = victory ? GamePhase.RunComplete : GamePhase.GameOver;
        Debug.Log(victory ? "Run completed successfully!" : "Run failed. Game over.");
    }
}
