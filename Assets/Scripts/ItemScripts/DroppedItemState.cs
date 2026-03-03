// DroppedItemState.cs
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Lives on world items that have been dropped. Persists current durability.
/// </summary>
public class DroppedItemState : MonoBehaviour
{
    public ItemData itemData;
    public int currentDurability;
}

