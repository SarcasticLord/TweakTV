// InventoryManager.cs
using NUnit.Framework.Interfaces;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ItemInstance[] inventory = new ItemInstance[3];
    public int selectedIndex = 0;
    public HotbarUI hotbarUI; // Assign in Inspector
    public GameObject player;
    public GameObject itemSpawn;


    private void Start()
    {
        UpdateHotbarUI(); // Call this to refresh UI
    }

    public void AddItem(ItemData itemData)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = new ItemInstance(itemData);
                return;
            }
        }

        Debug.Log("Inventory full!");
    }

    public void SwitchItem(int direction)
    {
        selectedIndex = (selectedIndex + direction + inventory.Length) % inventory.Length;
        Debug.Log($"Switched to slot {selectedIndex}");
        // Update UI here
        UpdateHotbarUI(); // Call this to refresh UI
    }

    private void Update()
    {

        // Switch item with Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchItem(1); // Cycle forward
        }

        // Direct select with number keys
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedIndex = 2;

        // Use item with E
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseSelectedItem();
        }
    }

    void UpdateHotbarUI()
    {
        if (hotbarUI != null)
        {
            hotbarUI.UpdateUI(inventory, selectedIndex);
        }
        Debug.Log("Inventory updated!");
    }

    public void UseSelectedItem()
    {
        var item = inventory[selectedIndex];
        if (item != null)
        {
            item.Use(player);
            UpdateHotbarUI();
            if (item.IsBroken)
            { 
                Debug.Log($"{item.data.itemName} has broken and is removed from inventory.");
                inventory[selectedIndex] = null;
            }
        }
    }
    public void PickUpItem(ItemData itemData, GameObject instance = null)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = new ItemInstance(itemData, instance);
                selectedIndex = i;
                Debug.Log($"Picked up {itemData.itemName} into slot {i}");
                
                UpdateHotbarUI();
                return;
            }
        }

        Debug.Log("Inventory full! Cannot pick up item.");
    }

}
