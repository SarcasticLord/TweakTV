// InventoryManager.cs
using NUnit.Framework.Interfaces;
using System.ComponentModel.Design;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ItemInstance[] inventory = new ItemInstance[3];
    public int selectedIndex;
    public HotbarUI hotbarUI; // Assign in Inspector
    public GameObject player;
    public GameObject inventoryContainer;
    public GameObject flashlight;
    private bool isOn;


    private void Start()
    {
        isOn = false;
        ToggleFlashlight();
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchItem(1); // Cycle forward
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight(); // Cycle forward
        }

        // Direct select with number keys
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            selectedIndex = 0;
            UpdateHotbarUI();
            Debug.Log("Switched to Item 1");
        } 
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedIndex = 1;
            UpdateHotbarUI();
            Debug.Log("Switched to Item 2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedIndex = 2;
            UpdateHotbarUI();
            Debug.Log("Switched to Item 3");
        }
        

        // Use item with LeftMouseClick
        if (Input.GetMouseButtonDown(0))
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

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i]?.instance != null)
            {
                inventory[i].instance.SetActive(i == selectedIndex);
            }
        }
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
                if (item.instance != null)
                {
                    Debug.Log("Destroying item instance...");
                    Destroy(item.instance);
                }
                inventory[selectedIndex] = null;
                UpdateHotbarUI();
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
    
    public void ToggleFlashlight()
    {
            isOn = !isOn;
            Light lightComponent = flashlight.GetComponentInChildren<Light>();
        Debug.Log($"Toggling Light... {lightComponent}");
            if (lightComponent != null)
            {
                lightComponent.enabled = isOn;
            }
    }
}


