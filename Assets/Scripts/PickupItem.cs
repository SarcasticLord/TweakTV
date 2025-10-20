// PickupItem.cs
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData; // Assign in Inspector
    private bool playerInRange = false;
    private InventoryManager inventoryManager;
    public GameObject itemPrefab;
    public GameObject pickupPrompt;
    public Transform itemSpawn;
    private Quaternion spawnRotation = Quaternion.Euler(90f, -10f, 0f); // Example: rotate 90° around Y-axis


    private void Start()
    {
        pickupPrompt.SetActive(false);
        if (pickupPrompt != null)
        {
            pickupPrompt.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            inventoryManager = other.GetComponent<InventoryManager>();
            if (pickupPrompt != null)
            {
                pickupPrompt.SetActive(true);
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
        pickupPrompt.SetActive(false);

    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            pickupPrompt.SetActive(false);
            if (inventoryManager != null && inventoryManager.hotbarUI != null)
                {
                    if (inventoryManager.hotbarUI.IsFull(inventoryManager.inventory))
                    {
                        Debug.Log("Inventory full. Cannot pick up item.");
                        return;
                    }
                    Destroy(gameObject);
                    GameObject instance = Instantiate(itemPrefab, itemSpawn.transform.position, itemSpawn.transform.rotation);
                    instance.transform.SetParent(itemSpawn, true);
                    instance.transform.localRotation = Quaternion.identity;
                    instance.transform.localPosition = Vector3.zero;
                    instance.SetActive(false);
                    Rigidbody rb = instance.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true; // Prevent physics interaction
                        rb.useGravity = false;
                    }

                    inventoryManager.PickUpItem(itemData, instance);
                    pickupPrompt.SetActive(false);

            }
        }
    }
}

