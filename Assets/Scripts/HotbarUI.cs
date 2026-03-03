using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class HotbarUI : MonoBehaviour
{
    [Header("Slot Settings")]
    public GameObject slotPrefab;
    public int slotCount = 3;

    [Header("References")]
    public InventoryManager inventoryManager;

    private List<Image> slotImages = new List<Image>();
    private List<TextMeshProUGUI> durabilityTexts = new List<TextMeshProUGUI>();
    //private List<Image> highlights = new List<Image>();

    void Start()
    {
        GenerateSlots();
        UpdateUI(inventoryManager.inventory, inventoryManager.selectedIndex);
    }

    void GenerateSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab, transform);
            Image icon = slot.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI durability = slot.transform.Find("DurabilityText").GetComponent<TextMeshProUGUI>();
            //Image highlight = slot.transform.Find("Highlight").GetComponent<Image>();

            slotImages.Add(icon);
            durabilityTexts.Add(durability);
            //highlights.Add(highlight);
        }
    }

    public void UpdateUI(ItemInstance[] inventory, int selectedIndex)
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (i < inventory.Length && inventory[i] != null)
            {
                slotImages[i].sprite = inventory[i].data.icon;
                slotImages[i].color = Color.white;
                durabilityTexts[i].text = inventory[i].currentDurability.ToString();
                Debug.Log($"Updating slot {i} with item {inventory[i]?.data?.itemName}");
            }
            else
            {
                slotImages[i].sprite = null;
                slotImages[i].color = new Color(1, 1, 1, 0);
                durabilityTexts[i].text = "";
            }

            //highlights[i].enabled = (i == selectedIndex);
        }
    }

    public bool IsFull(ItemInstance[] inventory)
    {
        foreach (var item in inventory)
        {
            if (item == null || item.currentDurability <= 0)
                return false;
        }
        return true;
    }
}