using UnityEngine;

[CreateAssetMenu(fileName = "Gear_", menuName = "Roguelike/Gear", order = 3)]
public class GearData : ScriptableObject
{
    [SerializeField] public string gearName;
    [SerializeField] public string description;
    [SerializeField] public GearSlot slot;
    [SerializeField] public Sprite gearIcon;

    // Stats this gear provides
    [SerializeField] public int maxHPBonus = 0;
    [SerializeField] public int damageBonus = 0;
    [SerializeField] public int armorBonus = 0;
    [SerializeField] public int energyBonus = 0;

    // Visual customization
    [SerializeField] public Material customMaterial; // Optional visual modifier

    public enum GearSlot
    {
        Head,
        Body,
        Hands,
        Feet,
        Accessory1,
        Accessory2
    }
}

/// <summary>
/// Player's equipped gear
/// </summary>
public class GearSet
{
    public GearData[] EquippedGear = new GearData[6];

    public int GetTotalStatBonus(string statName)
    {
        int total = 0;
        foreach (var gear in EquippedGear)
        {
            if (gear == null) continue;

            switch (statName)
            {
                case "MaxHP":
                    total += gear.maxHPBonus;
                    break;
                case "Damage":
                    total += gear.damageBonus;
                    break;
                case "Armor":
                    total += gear.armorBonus;
                    break;
                case "Energy":
                    total += gear.energyBonus;
                    break;
            }
        }
        return total;
    }
}
