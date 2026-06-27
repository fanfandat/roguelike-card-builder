using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardUIDisplay : MonoBehaviour
{
    [SerializeField] private Transform cardContainer;
    [SerializeField] private GameObject cardPrefab;

    /// <summary>
    /// Display card options for selection
    /// </summary>
    public void DisplayCardOptions()
    {
        // TODO: Implement card UI display
        // - Get card options from DeckManager
        // - Instantiate UI cards
        // - Add click handlers
        Debug.Log("Displaying card options");
    }

    /// <summary>
    /// Display player's hand
    /// </summary>
    public void DisplayHand(List<CardInstance> hand)
    {
        // TODO: Implement hand display
        Debug.Log($"Displaying hand with {hand.Count} cards");
    }
}
