
using UnityEngine;

[CreateAssetMenu(fileName = "KeycardBehavior", menuName = "Inventory/Behaviors/Keycard")]
public class KeycardBehavior : ItemBehavior
{
    public override void Use(ItemUseContext ctx)
    {
        var cam = ctx.camera;
        var go = ctx.instance.worldObject;
        if (!cam || !go) return;

        // Disable own colliders temporarily (avoid self-hits)
        var box = go.GetComponent<BoxCollider>();
        var sphere = go.GetComponent<SphereCollider>();
        if (box) box.enabled = false;
        if (sphere) sphere.enabled = false;

        var origin = cam.position;
        var dir = cam.forward;

        Debug.DrawRay(origin, dir * ctx.maxDistance, Color.green, 1f);

        if (Physics.Raycast(origin, dir, out var hit, ctx.maxDistance))
        {
            if (hit.collider.CompareTag("KeyPad"))
            {
                var rend = hit.collider.GetComponent<Renderer>();
                if (rend != null && rend.materials.Length >= 3)
                {
                    var mats = rend.materials;
                    mats[2].color = Color.green;
                    rend.materials = mats;
                }

                hit.collider.tag = "Untagged";
                ctx.instance.ConsumeDurability(1);
            }
        }
    }
}
