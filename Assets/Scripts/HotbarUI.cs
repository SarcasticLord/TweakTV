// HotbarUI.cs
using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{
    public Image[] slotImages; // Assign in Inspector
    public Image[] slotHighlights; // Optional: highlight borders
    public Text[] durabilityTexts; // Optional: show durability

    public void UpdateUI(ItemInstance[] inventory, int selectedIndex)
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (inventory[i] != null)
            {
                slotImages[i].sprite = inventory[i].data.icon;
                slotImages[i].color = Color.blue;
                durabilityTexts[i].text = inventory[i].currentDurability.ToString();
            }
            else
            {
                slotImages[i].sprite = null;
                slotImages[i].color = Color.red; // Transparent
                durabilityTexts[i].text = "";
            }

            slotHighlights[i].enabled = (i == selectedIndex);
        }
    }
}
