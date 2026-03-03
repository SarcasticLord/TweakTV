
using System;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Runtime state for an item the player is holding/using. Keeps durability and cooldowns,
/// and delegates to the assigned ItemBehavior on Use.
/// </summary>
public class ItemInstanceV2
{
    public ItemData data;
    public int currentDurability;
    public GameObject worldObject; // The item GameObject in the scene (prefab instance)
    public GameObject instance;    // Optional reference (if different from worldObject)

    private bool canUseSkill = true;
    private bool canUseWeapon = true;

    public ItemInstanceV2(ItemData itemData, GameObject instance = null, GameObject hitbox = null)
    {
        data = itemData;
        currentDurability = itemData != null ? itemData.maxDurability : 0;
        this.instance = instance;
        worldObject = instance;
    }

    public bool IsBroken => currentDurability <= 0;
    public bool CanUseSkill() => canUseSkill && !IsBroken;
    public bool CanUseWeapon() => canUseWeapon && !IsBroken;

    /// <summary>
    /// Main entry point: delegate to the item's behavior.
    /// </summary>
    public void Use(ItemUseContext ctx)
    {
        if (IsBroken)
        {
            Debug.Log($"{data.itemName} is broken or out of uses.");
            return;
        }

        // Optionally set chat mode based on type
        if (ctx.chat != null)
        {
            if (data.itemType == ItemType.Weapon) ctx.chat.ChangeChatSource("Combat");
            else ctx.chat.ChangeChatSource("Chatw");
        }

        data.behavior?.Use(ctx);
    }

    // ——— Helpers ———

    public void ConsumeDurability(int amount)
    {
        currentDurability = Mathf.Max(0, currentDurability - amount);
    }

    public async void StartSkillCooldown(int milliseconds, Action onStart, Action onEnd)
    {
        if (!canUseSkill) return;
        canUseSkill = false;

        Debug.Log("Skill used!");
        onStart?.Invoke();

        await Task.Delay(milliseconds);

        onEnd?.Invoke();
        canUseSkill = true;
        Debug.Log("Skill is ready again!");
    }

    public async void StartWeaponCooldown(int milliseconds)
    {
        if (!canUseWeapon) return;
        canUseWeapon = false;

        Debug.Log("Weapon used!");

        await Task.Delay(milliseconds);

        canUseWeapon = true;
        ConsumeDurability(1);
        Debug.Log("Weapon is ready again!");
    }

    public void SpawnHitboxBat(GameObject user)
    {
        if (data.itemType != ItemType.Weapon || data.hitbox == null)
        {
            Debug.LogWarning($"No hitbox assigned for weapon: {data?.itemName}");
            return;
        }

        var spawn = GetDescendantByTag(worldObject, "HitboxSpawn");
        if (!spawn)
        {
            Debug.LogWarning("HitboxSpawn not found on weapon.");
            return;
        }

        var spawnedHitbox = GameObject.Instantiate(data.hitbox, spawn.position, Quaternion.identity, spawn);
        var hitboxScript = spawnedHitbox.GetComponent<WeaponHitbox>();
        if (hitboxScript != null && user != null)
        {
            hitboxScript.forceDirection = user.transform.forward;
        }

        GameObject.Destroy(spawnedHitbox, 0.5f);
        Debug.Log($"Spawned hitbox for {data.itemName} at {spawn.position}");
    }

    private Transform GetDescendantByTag(GameObject parent, string tag)
    {
        if (!parent) return null;
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag(tag)) return child;
        }
        return null;
    }
}