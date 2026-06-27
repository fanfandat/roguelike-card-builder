using UnityEngine;
using System.Collections.Generic;

public class BaseManager : MonoBehaviour
{
    [SerializeField] private List<StructureInstance> structures = new List<StructureInstance>();
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
    /// Add resources from a completed run
    /// </summary>
    public void AddRunResources(int amount)
    {
        totalResources += amount;
        Debug.Log($"Added {amount} resources. Total: {totalResources}");
    }

    /// <summary>
    /// Build a new structure
    /// </summary>
    public void BuildStructure(StructureData structureData)
    {
        StructureInstance newStructure = new StructureInstance(structureData);
        structures.Add(newStructure);
        Debug.Log($"Built structure: {structureData.structureName}");
    }

    /// <summary>
    /// Level up a structure
    /// </summary>
    public void LevelUpStructure(StructureInstance structure)
    {
        if (totalResources >= structure.Data.resourcesRequiredPerLevel)
        {
            totalResources -= structure.Data.resourcesRequiredPerLevel;
            structure.LevelUp();
            Debug.Log($"Leveled up {structure.Data.structureName} to level {structure.CurrentLevel}");
        }
        else
        {
            Debug.LogWarning("Not enough resources to level up structure");
        }
    }

    /// <summary>
    /// Get all structures
    /// </summary>
    public List<StructureInstance> GetStructures() => structures;

    /// <summary>
    /// Get total resources
    /// </summary>
    public int GetTotalResources() => totalResources;
}
