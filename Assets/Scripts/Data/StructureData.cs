using UnityEngine;

[CreateAssetMenu(fileName = "Structure_", menuName = "Roguelike/Structure", order = 4)]
public class StructureData : ScriptableObject
{
    [SerializeField] public string structureName;
    [SerializeField] public string description;
    [SerializeField] public Sprite structureIcon;

    // Resources granted per run
    [SerializeField] public int resourcesPerRun = 10;

    // Leveling
    [SerializeField] public int maxLevel = 10;
    [SerializeField] public int resourcesRequiredPerLevel = 50;

    // Benefits
    [SerializeField] public float cardDropRateBonus = 0f;    // % bonus to card drops
    [SerializeField] public float gearDropRateBonus = 0f;    // % bonus to gear drops
    [SerializeField] public float startingHealthBonus = 0f;  // % bonus
    [SerializeField] public float difficultyReduction = 0f;  // % reduction
}

/// <summary>
/// Player's base structure instance
/// </summary>
public class StructureInstance
{
    public StructureData Data { get; set; }
    public int CurrentLevel { get; set; } = 1;
    public int ResourcesAccumulated { get; set; } = 0;

    public StructureInstance(StructureData data)
    {
        Data = data;
    }

    public bool CanLevelUp()
    {
        return CurrentLevel < Data.maxLevel &&
               ResourcesAccumulated >= Data.resourcesRequiredPerLevel;
    }

    public void LevelUp()
    {
        if (CanLevelUp())
        {
            ResourcesAccumulated -= Data.resourcesRequiredPerLevel;
            CurrentLevel++;
        }
    }
}
