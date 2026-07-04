using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages base buildings and persistent progression between runs.
/// Persists across scenes using DontDestroyOnLoad.
/// </summary>
public class BaseManager : MonoBehaviour
{
    // Runtime list - not serialized (StructureInstance is not serializable)
    private List<StructureInstance> structures = new List<StructureInstance>();

    // Serialized - total resources is a primitive type
    [SerializeField] private int totalResources = 0;

    public static BaseManager Instance { get; private set; }

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
    /// Add resources earned from a completed run
    /// </summary>
    public void AddRunResources(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot add negative resources");
            return;
        }

        totalResources += amount;
        Debug.Log($"Added {amount} resources. Total: {totalResources}");
    }

    /// <summary>
    /// Build a new structure at the base
    /// </summary>
    public void BuildStructure(StructureData structureData)
    {
        if (structureData == null)
        {
            Debug.LogError("Cannot build null structure");
            return;
        }

        StructureInstance newStructure = new StructureInstance(structureData);
        structures.Add(newStructure);
        Debug.Log($"Built structure: {structureData.structureName}");
    }

    /// <summary>
    /// Level up an existing structure if enough resources
    /// </summary>
    public bool LevelUpStructure(StructureInstance structure)
    {
        if (structure == null)
        {
            Debug.LogError("Cannot level up null structure");
            return false;
        }

        if (totalResources >= structure.Data.resourcesRequiredPerLevel)
        {
            totalResources -= structure.Data.resourcesRequiredPerLevel;
            structure.LevelUp();
            Debug.Log($"Leveled up {structure.Data.structureName} to level {structure.CurrentLevel}. Resources: {totalResources}");
            return true;
        }
        else
        {
            Debug.LogWarning($"Not enough resources to level up. Have: {totalResources}, Need: {structure.Data.resourcesRequiredPerLevel}");
            return false;
        }
    }

    /// <summary>
    /// Get all structures at the base
    /// </summary>
    public List<StructureInstance> GetStructures() => new List<StructureInstance>(structures);

    /// <summary>
    /// Get total resources
    /// </summary>
    public int GetTotalResources() => totalResources;
}
