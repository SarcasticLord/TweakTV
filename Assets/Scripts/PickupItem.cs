
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


    private void Start()
    {
        GameObject crosshairObject = GameObject.Find("Crosshair");
        pickupPrompt = crosshairObject.GetComponent<Image>();
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
        pickupPrompt.sprite = crosshair;

    }

    private void Update()
    {
        if (playerInRange == true) {
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
                    pickupPrompt.sprite = crosshair;
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

