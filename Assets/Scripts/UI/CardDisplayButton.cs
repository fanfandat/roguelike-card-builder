using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI component for displaying a single card in the hand
/// </summary>
public class CardDisplayButton : MonoBehaviour
{
    private CardInstance card;
    private CombatSystem combatSystem;
    private Button button;
    private Text cardNameText;
    private Text cardCostText;
    private Text cardDescriptionText;

    private void Awake()
    {
        button = GetComponent<Button>();
        cardNameText = transform.Find("CardName")?.GetComponent<Text>();
        cardCostText = transform.Find("CardCost")?.GetComponent<Text>();
        cardDescriptionText = transform.Find("CardDescription")?.GetComponent<Text>();
    }

    public void Initialize(CardInstance cardInstance, CombatSystem combatSys)
    {
        card = cardInstance;
        combatSystem = combatSys;

        // Update UI
        if (cardNameText != null)
            cardNameText.text = card.Data.cardName;
        if (cardCostText != null)
            cardCostText.text = $"Cost: {card.Data.cost}";
        if (cardDescriptionText != null)
            cardDescriptionText.text = card.Data.description;

        // Add click listener
        if (button != null)
            button.onClick.AddListener(OnCardClicked);
    }

    private void OnCardClicked()
    {
        if (combatSystem != null)
        {
            Debug.Log($"Card clicked: {card.Data.cardName}");
            combatSystem.PlayCard(card);
        }
    }
}
