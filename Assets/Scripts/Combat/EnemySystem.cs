using UnityEngine;
using System.Collections.Generic;

public class EnemySystem : MonoBehaviour
{
    [SerializeField] private List<EnemyData> enemyPool = new List<EnemyData>();
    [SerializeField] private int bossCount = 7; // 7 bosses per run

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

    private void LoadEnemiesFromResources()
    {
        EnemyData[] loadedEnemies = Resources.LoadAll<EnemyData>("Enemies");
        enemyPool.AddRange(loadedEnemies);
        Debug.Log($"Loaded {enemyPool.Count} enemy types");
    }

    /// <summary>
    /// Generate a random enemy encounter (scaled by player count)
    /// </summary>
    public List<EnemyInstance> GenerateEncounter(int encounterLevel)
    {
        List<EnemyInstance> encounter = new List<EnemyInstance>();
        float difficultyScalar = GameState.Instance.DifficultyScalar;

        // Simple: 1-3 enemies based on difficulty
        int enemyCount = Mathf.Min(1 + encounterLevel / 3, 3);
        enemyCount = Mathf.CeilToInt(enemyCount * difficultyScalar);

        for (int i = 0; i < enemyCount; i++)
        {
            EnemyData enemyData = GetRandomEnemy();
            EnemyInstance enemy = new EnemyInstance(enemyData);

            // Scale enemy stats by difficulty
            enemy.CurrentHP = Mathf.CeilToInt(enemy.CurrentHP * difficultyScalar);

            encounter.Add(enemy);
        }

        return encounter;
    }

    /// <summary>
    /// Generate a boss encounter
    /// </summary>
    public EnemyInstance GenerateBoss(int bossNumber)
    {
        // TODO: Implement boss generation
        // For now, just return a stronger enemy
        EnemyData bossData = GetRandomEnemy();
        EnemyInstance boss = new EnemyInstance(bossData);
        boss.CurrentHP = Mathf.CeilToInt(boss.CurrentHP * 2.5f * GameState.Instance.DifficultyScalar);
        return boss;
    }

    private EnemyData GetRandomEnemy()
    {
        return enemyPool[Random.Range(0, enemyPool.Count)];
    }
}
