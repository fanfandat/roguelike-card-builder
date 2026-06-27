using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }

    [SerializeField] private int maxPlayers = 4;

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

    /// <summary>
    /// Start a local game with specified player count
    /// </summary>
    public void StartLocalGame(int playerCount)
    {
        playerCount = Mathf.Clamp(playerCount, 1, maxPlayers);
        GameState.Instance.SetPlayerCount(playerCount);
        GameState.Instance.StartNewRun();
        Debug.Log($"Local game started with {playerCount} players");
    }

    /// <summary>
    /// Start an online multiplayer session
    /// TODO: Implement actual networking with Netcode for GameObjects or Mirror
    /// </summary>
    public void StartOnlineGame(int playerCount)
    {
        playerCount = Mathf.Clamp(playerCount, 1, maxPlayers);
        // TODO: Connect to server, instantiate network objects
        Debug.Log($"Online game started with {playerCount} players (TODO: implement networking)");
    }
}
