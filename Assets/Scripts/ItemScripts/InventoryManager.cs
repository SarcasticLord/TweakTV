
// InventoryManager.cs
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ItemInstance[] inventory = new ItemInstance[3];
    public int selectedIndex;

    [Header("UI/Player")]
    public HotbarUI hotbarUI;            // Assign in Inspector
    public GameObject player;            // Player root
    public ItemEquipSystem equipSystem;  // Assign in Inspector (on Player)

    [Header("Flashlight (if you keep this toggler)")]
    public GameObject flashlight;
    private bool isOn;

    private void Start()
    {
        isOn = false;
        ToggleFlashlight();
        UpdateHotbarUI();
    }

    public void AddItem(ItemData itemData)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = new ItemInstance(itemData);
                UpdateHotbarUI();
                return;
            }
        }
        Debug.Log("Inventory full!");
    }

    public void SwitchItem(int direction)
    {
        selectedIndex = (selectedIndex + direction + inventory.Length) % inventory.Length;
        Debug.Log($"Switched to slot {selectedIndex}");
        UpdateHotbarUI();
    }

    private void Update()
    {
        //Drop Item
        if (Input.GetKeyDown(KeyCode.G)) DropSelectedItem();

        //Cycle Items
        if (Input.GetKeyDown(KeyCode.Tab)) SwitchItem(1);

        //Toggle Flashlight
        if (Input.GetKeyDown(KeyCode.F)) ToggleFlashlight();

        //Quick Switch
        if (Input.GetKeyDown(KeyCode.Alpha1)) { selectedIndex = 0; UpdateHotbarUI(); Debug.Log("Switched to Item 1"); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { selectedIndex = 1; UpdateHotbarUI(); Debug.Log("Switched to Item 2"); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { selectedIndex = 2; UpdateHotbarUI(); Debug.Log("Switched to Item 3"); }

        if (Input.GetMouseButtonDown(0)) UseSelectedItem();
    }

    void UpdateHotbarUI()
    {
        // UI refresh
        if (hotbarUI != null) hotbarUI.UpdateUI(inventory, selectedIndex);
        Debug.Log("Inventory updated!");

        // Enable only the selected item's instance; disable others
        for (int i = 0; i < inventory.Length; i++)
        {
            var inst = inventory[i];
            if (inst?.instance != null)
            {
                bool active = i == selectedIndex;
                inst.instance.SetActive(active);

                // (Optional) Re-apply offsets when item becomes active,
                // in case something changed or you switched slots
                if (active && equipSystem != null)
                {
                    equipSystem.Equip(inst);
                }
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

    /// <summary>
    /// Called when picking up an item from the world.
    /// </summary>
    public void PickUpItem(ItemData itemData, GameObject instance = null)
    {
        // Find an empty slot
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = new ItemInstance(itemData, instance);
                selectedIndex = i;
                Debug.Log($"Picked up {itemData.itemName} into slot {i}");

                // Equip to the single hold point with per-item offsets
                if (equipSystem != null && instance != null)
                {
                    equipSystem.Equip(inventory[i]);
                }

                UpdateHotbarUI();
                return;
            }
        }

        Debug.Log("Inventory full! Cannot pick up item.");
    }

    public void ToggleFlashlight()
    {
        isOn = !isOn;
        Light lightComponent = flashlight ? flashlight.GetComponentInChildren<Light>() : null;
        Debug.Log($"Toggling Light... {lightComponent}");
        if (lightComponent != null)
        {
            lightComponent.enabled = isOn;
        }
    }
    
// InventoryManager.cs (additions)

    // ... your existing fields ...

    [Header("Drop Settings")]
    [SerializeField] private float dropDistance = 1.25f;
    [SerializeField] private float dropForwardImpulse = 2.5f;
    [SerializeField] private float dropUpImpulse = 1.0f;
    /// <summary>
    /// Drops the currently selected item into the world.
    /// </summary>
    public void DropSelectedItem()
    {
        var item = inventory[selectedIndex];
        if (item == null || item.instance == null)
        {
            Debug.Log("No item to drop.");
            return;
        }

        // Perform the drop
        DropItemInstance(item);

        // Remove from inventory & update UI
        inventory[selectedIndex] = null;

        // Optionally auto-select next non-null slot
        SelectNextAvailableSlot();

        UpdateHotbarUI();
    }

    /// <summary>
    /// Handles unparenting, physics enabling, animator disabling, and positioning.
    /// Keeps the instance's durability/state intact.
    /// </summary>
    private void DropItemInstance(ItemInstance item)
    {
        GameObject go = item.instance;

        // Unparent from the hold point
        go.transform.SetParent(null, true);

        // Position in front of the player/camera
        Transform cam = Camera.main ? Camera.main.transform : player.transform;
        Vector3 startPos = cam.position + cam.forward * dropDistance;

        go.transform.position = startPos;
        go.transform.rotation = Quaternion.LookRotation(cam.forward, Vector3.up);

        // Re-enable physics
        var rb = go.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            // Small toss forward
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(cam.forward * dropForwardImpulse + Vector3.up * dropUpImpulse, ForceMode.VelocityChange);
        }

        // Re-enable colliders
        foreach (var col in go.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }

        // Disable Animator if present (so it behaves like a loose prop)
        var animator = go.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }

        // Persist durability to the dropped object so it can be picked back up
        var state = go.GetComponent<DroppedItemState>();
        if (state == null) state = go.AddComponent<DroppedItemState>();
        state.itemData = item.data;
        state.currentDurability = item.currentDurability;

        // Make the object visible in world (it was active only in selected slot)
        go.SetActive(true);

        Debug.Log($"Dropped {item.data.itemName} with durability {item.currentDurability}");
    }

    private void SelectNextAvailableSlot()
    {
        // Try forward then wrap
        for (int i = 0; i < inventory.Length; i++)
        {
            int idx = (selectedIndex + i) % inventory.Length;
            if (inventory[idx] != null)
            {
                selectedIndex = idx;
                return;
            }
        }
        // No items left; keep index and UI will show empty
    }

    public void PickUpItemWithDurability(ItemData itemData, GameObject instance, int currentDurability)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                var ii = new ItemInstance(itemData, instance);
                ii.currentDurability = Mathf.Clamp(currentDurability, 0, itemData.maxDurability);
                inventory[i] = ii;
                selectedIndex = i;

                // Equip into hold point if present
                if (equipSystem != null && instance != null)
                {
                    equipSystem.Equip(ii);
                }

                UpdateHotbarUI();
                Debug.Log($"Picked up {itemData.itemName} with durability {ii.currentDurability} into slot {i}");
                return;
            }
        }
        Debug.Log("Inventory full! Cannot pick up item.");
    }
}
