
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData; // Assign in Inspector
    private bool playerInRange = false;
    private InventoryManager inventoryManager;
    public GameObject itemPrefab;
    private Image pickupPrompt;
    public Sprite crosshair;
    public Sprite pickup;
    public Transform itemSpawn;
    private Quaternion spawnRotation = Quaternion.Euler(90f, -10f, 0f); // Example: rotate 90° around Y-axis


    private void Start()
    {
        pickupPrompt = FindObjectOfType<Crosshair>;
        playerInRange = false;
        if (pickupPrompt != null)
        {
            pickupPrompt.sprite = crosshair;
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
        pickupPrompt.sprite = crosshair;

    }

    private void Update()
    {
        if (playerInRange == false)
        {
            pickupPrompt.sprite = crosshair;
        }
        else if (playerInRange == true) {
            pickupPrompt.sprite = pickup;
            if (playerInRange && Input.GetMouseButtonDown(1))
            {
                if (inventoryManager != null && inventoryManager.hotbarUI != null)
                {
                    if (inventoryManager.hotbarUI.IsFull(inventoryManager.inventory))
                    {
                        Debug.Log("Inventory full. Cannot pick up item.");
                        return;
                    }
                    Destroy(gameObject);
                    playerInRange = false;
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
                }
            }
        }
    }
}

