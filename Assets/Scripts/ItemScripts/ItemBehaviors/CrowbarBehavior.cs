
using UnityEngine;

[CreateAssetMenu(fileName = "CrowbarBehavior", menuName = "Inventory/Behaviors/Crowbar")]
public class CrowbarBehavior : ItemBehavior
{
    public override void Use(ItemUseContext ctx)
    {
        var cam = ctx.camera;
        var go = ctx.instance.worldObject;
        if (!cam || !go) return;

        // Disable own colliders temporarily (avoid self-hits)
        var box = go.GetComponent<BoxCollider>();
        if (box) box.enabled = false;

        var origin = cam.position;
        var dir = cam.forward;

        Debug.DrawRay(origin, dir * ctx.maxDistance, Color.green, 1f);

        if (Physics.Raycast(origin, dir, out var hit, ctx.maxDistance))
        {
            if (hit.collider.CompareTag("Crowable"))
            {
                Rigidbody rb = hit.collider.attachedRigidbody;
                rb.constraints = RigidbodyConstraints.None;
                hit.collider.tag = "Untagged";
                ctx.instance.ConsumeDurability(1);
            }
        }

        // Optional: re-enable own colliders
        if (box) box.enabled = true;
    }
}
