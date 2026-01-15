using UnityEngine;

/// <summary>
/// Static data for each item kind (name, icon, durability, behavior, highlight color).
/// </summary>
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Identity")]
    public string itemName;
    public Sprite icon;
    public ItemType itemType;

    [Header("Durability & Stack")]
    public int maxDurability = 1;
    public int maxItems = 1;

    [Header("Combat")]
    public GameObject hitbox;

    [Header("Visuals")]
    [ColorUsage(true, true)] // Enable HDR in the Color field for glow-like highlights
    public Color highlightColor = Color.yellow;

    [Header("Behavior")]
    // Assign a concrete ItemBehavior asset here (Flashlight/Melee/Crowbar/Coffee/Keycard/etc.)
    public ItemBehavior behavior;


    [Header("Hold Point Offset")]

    public Vector3 holdLocalPosition;
    public Vector3 holdLocalRotation;
    public Vector3 holdLocalScale = Vector3.one;

}
