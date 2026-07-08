
using UnityEngine;

[CreateAssetMenu(fileName = "CoffeeBehavior", menuName = "Inventory/Behaviors/Coffee")]
public class CoffeeBehavior : ItemBehavior
{
    [SerializeField] private int cooldownMs = 10000;
    [SerializeField] private float buffSpeed = 25f;
    [SerializeField] private float normalSpeed = 7f;

    public override void Use(ItemUseContext ctx)
    {
        if (!ctx.instance.CanUseSkill()) return;

        ctx.instance.StartSkillCooldown(
            cooldownMs,
            onStart: () =>
            {
                if (ctx.fpc) ctx.fpc.walkSpeed = buffSpeed;
                if (ctx.coffeeVolume) ctx.coffeeVolume.enabled = true;
            },
            onEnd: () =>
            {
                if (ctx.fpc) ctx.fpc.walkSpeed = normalSpeed;
                if (ctx.coffeeVolume) ctx.coffeeVolume.enabled = false;
            });

        ctx.instance.ConsumeDurability(1);
    }
}