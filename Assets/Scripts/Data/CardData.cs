using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Card_", menuName = "Roguelike/Card", order = 1)]
public class CardData : ScriptableObject
{
    [SerializeField] public string cardName;
    [SerializeField] public string description;
    [SerializeField] public int cost; // Mana/energy cost
    [SerializeField] public CardType cardType;
    [SerializeField] public Sprite cardArt;

    // Card effects (simple version - expandable)
    [SerializeField] public int damageDealt = 0;
    [SerializeField] public int hpRestored = 0;
    [SerializeField] public int armorGained = 0;
    [SerializeField] public int energyGained = 0;

    // Rarity for balancing
    [SerializeField] public Rarity rarity = Rarity.Common;

    public enum CardType
    {
        Attack,
        Defense,
        Utility,
        Special
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
}

/// <summary>
/// Runtime card instance (deck contains CardInstances, not CardData)
/// </summary>
public class CardInstance
{
    public CardData Data { get; set; }
    public int InstanceID { get; set; }

    public CardInstance(CardData data, int id)
    {
        Data = data;
        InstanceID = id;
    }
}
