
using EasyPeasyFirstPersonController;
using UnityEngine;
using UnityEngine.InputSystem; // If you use the new Input System. Remove if using legacy input.

/// <summary>
/// Sits on the Player. Provides runtime references to behaviors via ItemUseContext,
/// and calls instance.Use(ctx) when the player uses the current item.
/// </summary>
public class PlayerItemUser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ChatDisplay chat;
    [SerializeField] private FirstPersonController fpc;
    [SerializeField] private UnityEngine.Rendering.Volume coffeeVolume;

    [Header("Setup")]
    [SerializeField] private Transform cam;
    [SerializeField] private float maxUseDistance = 5f;

    // The currently equipped item instance (managed by your inventory system)
    public ItemInstanceV2 currentItemInstance;

    private void Reset()
    {
        if (cam == null && Camera.main != null) cam = Camera.main.transform;
    }

    private void Update()
    {
        // Example: press Left Mouse to use current item
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            UseCurrentItem();
        }

        // Or legacy Input:
        // if (Input.GetMouseButtonDown(0)) { UseCurrentItem(); }
    }

    public void UseCurrentItem()
    {
        if (currentItemInstance == null) return;

        var ctx = new ItemUseContext
        {
            instance = currentItemInstance,
            user = gameObject,
            camera = cam != null ? cam : (Camera.main ? Camera.main.transform : null),
            chat = chat,
            fpc = fpc,
            coffeeVolume = coffeeVolume,
            maxDistance = maxUseDistance
        };


        currentItemInstance.Use(ctx);
    }
}