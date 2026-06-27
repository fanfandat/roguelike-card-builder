using UnityEngine;

public class GearManager : MonoBehaviour
{
    private GearSet currentGearSet = new GearSet();

    /// <summary>
    /// Equip gear to a slot
    /// </summary>
    public void EquipGear(GearData gear)
    {
        int slotIndex = (int)gear.slot;
        currentGearSet.EquippedGear[slotIndex] = gear;
        Debug.Log($"Equipped {gear.gearName} to {gear.slot}");
    }

    /// <summary>
    /// Get total stat bonuses from current gear
    /// </summary>
    public int GetStatBonus(string statName)
    {
        return currentGearSet.GetTotalStatBonus(statName);
    }

    /// <summary>
    /// Get current gear set
    /// </summary>
    public GearSet GetGearSet() => currentGearSet;

    public void ClearGear()
    {
        currentGearSet = new GearSet();
    }
}
