using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private CardUIDisplay cardUIDisplay;
    [SerializeField] private BaseUIDisplay baseUIDisplay;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// Show card selection UI
    /// </summary>
    public void ShowCardSelection()
    {
        if (cardUIDisplay != null)
            cardUIDisplay.DisplayCardOptions();
    }

    /// <summary>
    /// Show base UI
    /// </summary>
    public void ShowBaseUI()
    {
        if (baseUIDisplay != null)
            baseUIDisplay.DisplayBase();
    }
}
