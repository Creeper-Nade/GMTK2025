using UnityEngine;
using UnityEngine.EventSystems;

public class PotionClickHandler : MonoBehaviour, IPointerClickHandler
{
    public PotionItem potionItem;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (potionItem == null) return;

        if (!string.IsNullOrEmpty(potionItem.potionID))
        {
            // Toggle ∞¥≈•œ‘ æ
            bool isActive = potionItem.submitButton.activeSelf;
            potionItem.submitButton.SetActive(!isActive);
            potionItem.destroyButton.SetActive(!isActive);
        }
    }
}
