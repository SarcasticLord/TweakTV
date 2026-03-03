
using UnityEngine;

[CreateAssetMenu(fileName = "LightBehavior", menuName = "Inventory/Behaviors/Flashlight")]
public class LightBehavior : ItemBehavior
{
    public override void Use(ItemUseContext ctx)
    {
        var go = ctx.instance.worldObject;
        if (!go) return;

        // Assumes a child named "FlashlightLight"
        var lightTransform = go.transform.Find("Light");
        if (!lightTransform) return;

        var light = lightTransform.GetComponent<Light>();
        if (!light) return;

        light.enabled = !light.enabled;
        Debug.Log($"Flashlight toggled: {light.enabled}");
    }
}
