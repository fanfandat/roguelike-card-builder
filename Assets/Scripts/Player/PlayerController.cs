using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private string playerName = "Player 1";
    [SerializeField] private int startingHP = 50;

    private DeckManager deckManager;
    private GearManager gearManager;
    private PlayerCombatState combatState;

    private void Awake()
    {
        deckManager = GetComponent<DeckManager>();
        gearManager = GetComponent<GearManager>();
    }

    private void Start()
    {
        // Initialize player
        combatState = new PlayerCombatState
        {
            PlayerName = playerName,
            CurrentHP = startingHP,
            MaxHP = startingHP
        };
    }

    /// <summary>
    /// Start a new run for this player
    /// </summary>
    public void StartNewRun()
    {
        deckManager.ClearDeck();
        combatState.CurrentHP = combatState.MaxHP;
        combatState.CurrentArmor = 0;
        Debug.Log($"{playerName} started a new run");
    }

    public PlayerCombatState GetCombatState() => combatState;
    public DeckManager GetDeckManager() => deckManager;
    public GearManager GetGearManager() => gearManager;
}
