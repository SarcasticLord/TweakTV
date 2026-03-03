
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeaponBehavior", menuName = "Inventory/Behaviors/MeleeWeapon")]
public class MeleeWeaponBehavior : ItemBehavior
{
    [SerializeField] private int cooldownMs = 900;

    public override void Use(ItemUseContext ctx)
    {
        var go = ctx.instance.worldObject;
        if (!go) return;

        var animator = go.GetComponent<Animator>();
        if (animator == null) return;

        if (!ctx.instance.CanUseWeapon()) return;

        // Switch chat to combat (optional)
        ctx.chat?.ChangeChatSource("Combat");

        // Spawn weapon hitbox
        ctx.instance.SpawnHitboxBat(ctx.user);

        // Random attack
        int attack = Random.Range(0, 3);
        animator.SetTrigger(attack switch
        {
            0 => "Attack1",
            1 => "Attack2",
            _ => "Attack3"
        });

        // Start cooldown & consume durability when cooldown ends
        ctx.instance.StartWeaponCooldown(cooldownMs);
    }
}
