
using EasyPeasyFirstPersonController;
using UnityEngine;

/// <summary>
/// Base class for pluggable item behaviors. Create assets that inherit this.
/// </summary>
public abstract class ItemBehavior : ScriptableObject
{
    public abstract void Use(ItemUseContext ctx);
}

/// <summary>
/// Context passed to behaviors so they can act without using GameObject.Find.
/// Provide all runtime references here from your player/interaction systems.
/// </summary>
public class ItemUseContext
{
    public ItemInstanceV2 instance;
    public GameObject user;           // Player GameObject
    public Transform camera;          // Player camera transform
    public ChatDisplay chat;          // Optional (your HUD chat system)
    public FirstPersonController fpc; // Movement controller (for Coffee, etc.)
    public UnityEngine.Rendering.Volume coffeeVolume; // Post process (for Coffee)
    public float maxDistance = 5f;    // Raycast range or interaction range
}

