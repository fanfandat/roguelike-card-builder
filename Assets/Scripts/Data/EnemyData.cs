using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_", menuName = "Roguelike/Enemy", order = 2)]
public class EnemyData : ScriptableObject
{
    [SerializeField] public string enemyName;
    [SerializeField] public int maxHP = 10;
    [SerializeField] public int baseDamage = 2;
    [SerializeField] public int armor = 0;
    [SerializeField] public Sprite enemySprite;

    // AI behavior (simple)
    [SerializeField] public AIBehavior behavior = AIBehavior.Aggressive;

    public enum AIBehavior
    {
        Aggressive,  // Always attack
        Defensive,   // Build armor then attack
        Balanced,    // Mix of both
        Unpredictable // Random
    }
}

/// <summary>
/// Runtime enemy instance
/// </summary>
public class EnemyInstance
{
    public EnemyData Data { get; set; }
    public int CurrentHP { get; set; }
    public int CurrentArmor { get; set; }

    public EnemyInstance(EnemyData data)
    {
        Data = data;
        CurrentHP = data.maxHP;
        CurrentArmor = data.armor;
    }

    public void TakeDamage(int damage)
    {
        int armorReduction = Mathf.Min(CurrentArmor, damage);
        damage -= armorReduction;
        CurrentArmor -= armorReduction;

        CurrentHP -= Mathf.Max(1, damage); // Min 1 damage
    }

    public bool IsAlive => CurrentHP > 0;
}
