
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class PickupItem : MonoBehaviour
{
    [Header("Data")]
    public ItemData itemData;    // Assign in Inspector

    [Header("UI")]
    public Sprite crosshair;
    public Sprite pickup;

    [Header("Spawn (optional if you keep)")]
    public Transform itemSpawn;  // Can be unused now, since we don't instantiate

    private bool playerInRange = false;
    private InventoryManager inventoryManager;
    private Image pickupPrompt;

    private void Start()
    {
        var crosshairObject = GameObject.Find("Crosshair");
        pickupPrompt = crosshairObject ? crosshairObject.GetComponent<Image>() : null;
        playerInRange = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            inventoryManager = other.GetComponent<InventoryManager>();
            if (pickupPrompt != null)
            {
                pickupPrompt.sprite = pickup;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            inventoryManager = null;
        }
        if (pickupPrompt != null)
            pickupPrompt.sprite = crosshair;
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (pickupPrompt != null)
            pickupPrompt.sprite = pickup;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventoryManager == null || inventoryManager.hotbarUI == null)
                return;

            if (inventoryManager.hotbarUI.IsFull(inventoryManager.inventory))
            {
                Debug.Log("Inventory full. Cannot pick up item.");
                return;
            }

            // Determine durability to carry into inventory
            var droppedState = GetComponent<DroppedItemState>();
            int durabilityFromWorld = droppedState ? droppedState.currentDurability : itemData.maxDurability;

            // Prepare this world object to become the held instance
            PrepareForPickup(gameObject);

            // Hand off to InventoryManager: NO instantiation, preserve durability
            inventoryManager.PickUpItemWithDurability(itemData, gameObject, durabilityFromWorld);

            // Clear prompt/state
            playerInRange = false;
            if (pickupPrompt != null)
                pickupPrompt.sprite = crosshair;

            // Optional: remove DroppedItemState since it's now held
            if (droppedState != null)
            {
                Destroy(droppedState);
            }

            // Optional: disable this trigger so we don't re-fire while held
            var pickupCollider = GetComponent<Collider>();
            if (pickupCollider != null)
            {
                pickupCollider.enabled = false;
            }

            // NOTE: Do NOT Destroy(this.gameObject)
            // The InventoryManager/equip system now owns it and will parent it to the hold point,
            // and control active state based on selected slot.
        }
    }

    /// <summary>
    /// Disables physics and enables animator so the item behaves as a held prop.
    /// Colliders are disabled to avoid self-interaction while held.
    /// </summary>
    private void PrepareForPickup(GameObject go)
    {
        // Disable physics
        var rb = go.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Disable colliders while held (optional)
        foreach (var col in go.GetComponentsInChildren<Collider>())
        {
            // Keep the pickup trigger disabled, too
            col.enabled = false;
        }

        // Enable Animator for held animations (attack, flashlight toggle, etc.)
        var animator = go.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true;
        }

        // Let InventoryManager control visibility.
        // It will SetActive(true) only for the selected slot’s instance.
        go.SetActive(false);
    }
}
