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
    public Quaternion spawnRotation = Quaternion.Euler(0f, 90f, 0f); // Example: rotate 90° around Y-axis


    private void Start()
    {
        if (pickupPrompt != null)
        {
            pickupPrompt.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            inventoryManager = other.GetComponent<InventoryManager>();
            if (pickupPrompt != null){ 
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

        if (pickupPrompt != null) {
            pickupPrompt.SetActive(false);
    }

}

private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.G))
        {
            if (inventoryManager != null)
            {
                GameObject instance = Instantiate(itemPrefab, itemSpawn.transform.position, itemSpawn.transform.rotation);
                instance.transform.SetParent(itemSpawn, true);
                instance.transform.rotation = spawnRotation;
                Rigidbody rb = instance.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true; // Prevent physics interaction
                    rb.detectCollisions = false; // Optional: disable collisions
                    rb.useGravity = false;
                }

                inventoryManager.PickUpItem(itemData, instance);
                if (pickupPrompt != null)
                {
                    pickupPrompt.SetActive(false);
                }
                Destroy(gameObject); // Remove pickup from world
            }
        }
    }
}

