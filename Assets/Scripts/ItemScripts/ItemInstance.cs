using EasyPeasyFirstPersonController;
using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Unity;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = System.Random;

public class ItemInstance 
{
    public ItemData data;
    public int currentDurability;
    public GameObject player;
    public GameObject worldObject; // The actual flashlight GameObject in the scene
    public GameObject instance;
    private Transform camera = Camera.main.transform;
    private ChatDisplay chat;
    private float maxDistance = 5f;
    private bool canUseSkill = true;
    private bool canUseWeapon = true;
    private float cooldownTimer = 3;
    private FirstPersonController fpc;
    private Volume coffeeVolume;
    private GameObject visual = GameObject.Find("CoffeeVisual");
    private GameObject playerObject = GameObject.FindGameObjectWithTag("WholePlayer");

    public void Start()
    {

    }
    public bool IsBroken => currentDurability <= 0;

    public ItemInstance(ItemData itemData, GameObject instance = null, GameObject hitbox = null)
    {
        this.data = itemData;
        this.currentDurability = itemData.maxDurability;
        this.instance = instance;
        worldObject = instance;
    }



    public void Use(GameObject user)
    {
        GameObject chatobject = GameObject.Find("Chat");
        chat = chatobject.GetComponent<ChatDisplay>();
        if (currentDurability <= 0)
        {
            Debug.Log($"{data.itemName} is broken or out of uses.");
            return;
        }

        //Flashlight
        if (data.itemType == ItemType.Light && data.itemName == "Flashlight")
        {
            Transform lightTransform = worldObject.transform.Find("FlashlightLight");
            if (lightTransform != null)
            {
                Light light = lightTransform.GetComponent<Light>();
                if (light != null)
                {
                    light.enabled = !light.enabled; // Toggle light
                    Debug.Log($"Flashlight toggled: {light.enabled}");
                }
            }
        }

        //BaseballBat
        if (data.itemType == ItemType.Weapon)
        {
            Combat(); //Changes chat to combat
            Animator animator = worldObject.GetComponent<Animator>();
            int baseballCooldown = 900;
            // Trigger the animation
            if (animator != null)
            {
                if (canUseWeapon)
                    {
                        SpawnHitboxBat(user);
                        System.Random random = new();
                        int attack = random.Next(0, 3);
                        canUseWeapon = false;
                        if (attack == 0)
                        {
                        animator.SetTrigger("Attack1");
                        UseWeapon(baseballCooldown);
                        }
                        else if (attack == 1)
                        {
                        animator.SetTrigger("Attack2");
                        UseWeapon(baseballCooldown);

                        }
                        else if (attack == 2)
                        {
                        animator.SetTrigger("Attack3");
                        UseWeapon(baseballCooldown);
                        }
                        Debug.Log($"Played weapon animation for {data.itemName}");
                    }
            }
        }

        //Crowbar
        if (data.itemType == ItemType.Tool && data.itemName == "Crowbar")
        {
            if(camera != null)
            {
                MeshCollider collider = worldObject.GetComponent<MeshCollider>();
                BoxCollider collider2 = worldObject.GetComponent<BoxCollider>();
                Vector3 rayOrigin = camera.position;
                Vector3 rayDirection = camera.forward;

                collider2.enabled = false;
                collider.enabled = false;
                
                Debug.DrawRay(rayOrigin, rayDirection * 10f, Color.red, 10f);
                RaycastHit hit;
                if (Physics.Raycast(rayOrigin, rayDirection * 10f, out hit, maxDistance))
                {
                    if (hit.collider.CompareTag("Crowable"))
                    {
                        Collider targetCollider = hit.collider;
                        Rigidbody targetRb = targetCollider.GetComponent<Rigidbody>();

                        if (targetCollider != null)
                        {
                            targetCollider.enabled = true;
                        }

                        if (targetRb != null)
                        {
                            // Enable collider if needed
                            hit.collider.enabled = true;

                            // Remove constraints
                            targetRb.constraints = RigidbodyConstraints.None;
                            // Calculate direction from target to caster
                            Vector3 directionToCaster = (camera.position - hit.transform.position).normalized;
                            // Apply impulse force toward the caster
                            targetRb.AddForce(directionToCaster * 5, ForceMode.Impulse);
                            
                        }
                        UnityEngine.Object.Destroy(hit.collider.gameObject, 5f);
                        currentDurability--;
                    }
                }
            }
        }
        //Coffee
        if (data.itemType == ItemType.Consumable && data.itemName == "Coffee")
        {
            if (canUseSkill)
            {
                canUseSkill = false;
                coffeeVolume = visual.GetComponent<Volume>();
                fpc = playerObject.GetComponent<FirstPersonController>();
                int coffeeCooldown = 10000;
                UseItem(coffeeCooldown);
                currentDurability--;
            }
        }
        //Keycard
        if (data.itemType == ItemType.Weapon && data.itemName == "Keycard")
        {
            if (camera != null)
            {
                BoxCollider collider = worldObject.GetComponent<BoxCollider>();
                SphereCollider collider2 = worldObject.GetComponent<SphereCollider>();
                Vector3 rayOrigin = camera.position;
                Vector3 rayDirection = camera.forward;

                collider2.enabled = false;
                collider.enabled = false;

                Debug.DrawRay(rayOrigin, rayDirection * 10f, Color.red, 10f);
                RaycastHit hit;
                if (Physics.Raycast(rayOrigin, rayDirection * 10f, out hit, maxDistance))
                {
                    if (hit.collider.CompareTag("KeyPad"))
                    {

                        // Change the third material to red
                        Renderer renderer = hit.collider.GetComponent<Renderer>();
                        if (renderer != null && renderer.materials.Length >= 3)

                        {
                            Material[] mats = renderer.materials;
                            mats[2].color = Color.green; // Change color of third material
                            renderer.materials = mats;
                        }

                        // Remove the tag
                        hit.collider.tag = "Untagged";

                        // Destroy after 5 seconds

                        currentDurability--;

                    }
                }
            }
        }

        //Future Items
    }


    async void UseItem(int cooldownMilliseconds)
    {
            Debug.Log("Skill used!");
            fpc.walkSpeed = 25f;
            coffeeVolume.enabled = true;
            await StartCooldown(cooldownMilliseconds);
            fpc.walkSpeed = 7f;
            coffeeVolume.enabled = false; 
            canUseSkill = true;
            Debug.Log("Skill is ready again!");
    }
    async void UseWeapon(int cooldownMilliseconds)
    {
        Debug.Log("Weapon used!");
        await StartCooldown(cooldownMilliseconds);
        canUseWeapon = true;
        currentDurability--;
        Debug.Log("Skill is ready again!");
    }

    async Task StartCooldown(int milliseconds)
    {
        await Task.Delay(milliseconds);
    }

    public void Combat()
    {
        chat.ChangeChatSource("Combat");
    }

    public void Idle()
    {
        chat.ChangeChatSource("Chatw");
    }

    public void SpawnHitboxBat(GameObject user)
    {
        if (data.itemType != ItemType.Weapon || data.hitbox == null)
        {
            Debug.LogWarning($"No hitbox assigned for weapon: {data.itemName}");
            return;
        }
        Transform hitboxSpawn = GetDescendantByTag(worldObject, "HitboxSpawn");
        Debug.Log($"HitboxSpawnSuccessful");

        Vector3 spawnPosition = hitboxSpawn.position;
        GameObject spawnedHitbox = GameObject.Instantiate(data.hitbox, spawnPosition, Quaternion.identity);
        spawnedHitbox.transform.SetParent(hitboxSpawn, true);
        Debug.Log($"Spawned hitbox for {data.itemName} at {spawnPosition}");

        WeaponHitbox hitboxScript = spawnedHitbox.GetComponent<WeaponHitbox>();
        if (hitboxScript != null)
        {
            hitboxScript.forceDirection = user.transform.forward;
        }

        GameObject.Destroy(spawnedHitbox, 0.5f);
    }


    Transform GetDescendantByTag(GameObject parent, string tag)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag(tag))
            {
                return child;
            }
        }
        return null;
    }

}
