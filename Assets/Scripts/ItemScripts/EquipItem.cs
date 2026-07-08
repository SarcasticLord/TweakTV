
using UnityEngine;

/// <summary>
/// Equips items to the single hold point and applies per-item local offsets.
/// </summary>
public class ItemEquipSystem : MonoBehaviour
{
    [Header("Single Hold Point")]
    public Transform holdPoint; // Drag your single ItemHoldPoint here

    /// <summary>
    /// Parent the item's worldObject to the hold point and apply data offsets.
    /// </summary>
    public void Equip(ItemInstance instance)
    {
        if (instance == null || instance.worldObject == null || instance.data == null)
        {
            Debug.LogWarning("Equip failed: missing instance/worldObject/data.");
            return;
        }

        Transform t = instance.worldObject.transform;
        t.SetParent(holdPoint, false);

        // Apply per-item local offsets
        t.localPosition = instance.data.holdLocalPosition;
        t.localRotation = Quaternion.Euler(instance.data.holdLocalRotation);
        t.localScale = instance.data.holdLocalScale;

        // Disable physics for held items
        var rb = instance.worldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        foreach (var col in instance.worldObject.GetComponentsInChildren<Collider>())
        {
            col.enabled = false; // optional: disable colliders while held
        }
    }
}
