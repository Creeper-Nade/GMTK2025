using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public Text tooltipText;
    public GameObject tooltipPanel;

    public static ItemTooltip Instance;
    public static bool IsVisible { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
        HideTooltip();
    }

    public void ShowTooltip(string info, Vector2 position)
    {
        tooltipPanel.SetActive(true);
        tooltipText.text = info;
        tooltipPanel.transform.position = position;
        IsVisible = true;
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
        IsVisible = false;
    }
}

