using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages enemy spawning and encounter generation.
/// Scales enemy difficulty based on player count and encounter level.
/// </summary>
public class EnemySystem : MonoBehaviour
{
    // Number of bosses in a full run
    private const int BOSSES_PER_RUN = 7;

    // Runtime list - loaded from resources
    private List<EnemyData> enemyPool = new List<EnemyData>();

    public static EnemySystem Instance { get; private set; }

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
        LoadEnemiesFromResources();
    }

    /// <summary>
    /// Load all enemy types from Resources/Enemies folder
    /// </summary>
    private void LoadEnemiesFromResources()
    {
        EnemyData[] loadedEnemies = Resources.LoadAll<EnemyData>("Enemies");
        enemyPool.AddRange(loadedEnemies);
        Debug.Log($"Loaded {enemyPool.Count} enemy types");

        if (enemyPool.Count == 0)
        {
            Debug.LogWarning("No enemies found in Resources/Enemies!");
        }
    }

    /// <summary>
    /// Generate a random encounter scaled by difficulty
    /// </summary>
    public List<EnemyInstance> GenerateEncounter(int encounterLevel)
    {
        if (enemyPool.Count == 0)
        {
            Debug.LogError("Cannot generate encounter - no enemies loaded!");
            return new List<EnemyInstance>();
        }

        List<EnemyInstance> encounter = new List<EnemyInstance>();
        float difficultyScalar = GameState.Instance.DifficultyScalar;

        // 1-3 enemies based on difficulty
        int enemyCount = Mathf.Min(1 + encounterLevel / 3, 3);
        enemyCount = Mathf.CeilToInt(enemyCount * difficultyScalar);
        enemyCount = Mathf.Max(1, enemyCount); // At least 1 enemy

        for (int i = 0; i < enemyCount; i++)
        {
            EnemyData enemyData = GetRandomEnemy();
            EnemyInstance enemy = new EnemyInstance(enemyData);

            // Scale enemy stats by difficulty
            enemy.CurrentHP = Mathf.CeilToInt(enemy.CurrentHP * difficultyScalar);

            encounter.Add(enemy);
        }

        Debug.Log($"Generated encounter with {encounter.Count} enemies (Level {encounterLevel}, Difficulty {difficultyScalar:F2}x)");
        return encounter;
    }

    /// <summary>
    /// Generate a boss encounter (stronger than normal enemies)
    /// </summary>
    public EnemyInstance GenerateBoss(int bossNumber)
    {
        if (enemyPool.Count == 0)
        {
            Debug.LogError("Cannot generate boss - no enemies loaded!");
            return null;
        }

        EnemyData bossData = GetRandomEnemy();
        EnemyInstance boss = new EnemyInstance(bossData);
        boss.CurrentHP = Mathf.CeilToInt(boss.CurrentHP * 2.5f * GameState.Instance.DifficultyScalar);
        
        Debug.Log($"Generated boss #{bossNumber} with {boss.CurrentHP} HP");
        return boss;
    }

    /// <summary>
    /// Get a random enemy from the pool
    /// </summary>
    private EnemyData GetRandomEnemy()
    {
        return enemyPool[Random.Range(0, enemyPool.Count)];
    }
}
