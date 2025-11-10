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
    private GameObject playerObject = GameObject.Find("Player");

    public void Start()
    {
       
        Debug.Log($"fpc{fpc}");
        Debug.Log($"coffeeVolume{coffeeVolume}");

    }
    public bool IsBroken => currentDurability <= 0;

    public ItemInstance(ItemData itemData, GameObject instance = null, GameObject hitbox = null)
    {
        this.data = itemData;
        this.currentDurability = itemData.maxDurability;
        this.instance = instance;
        worldObject = instance;
    }

    public void SpawnHitbox(GameObject user)
    {
        if (data.itemType != ItemType.Weapon || data.hitbox == null)
        {
            Debug.LogWarning($"No hitbox assigned for weapon: {data.itemName}");
            return;
        }


        Vector3 spawnPosition = user.transform.position + user.transform.forward * data.offset;
        GameObject spawnedHitbox = GameObject.Instantiate(data.hitbox, spawnPosition, Quaternion.identity);
        spawnedHitbox.transform.SetParent(worldObject.transform, true);
        Debug.Log($"Spawned hitbox for {data.itemName} at {spawnPosition}");

        WeaponHitbox hitboxScript = spawnedHitbox.GetComponent<WeaponHitbox>();
        if (hitboxScript != null)
        {
            hitboxScript.forceDirection = user.transform.forward;
        }

        GameObject.Destroy(spawnedHitbox, 0.2f);
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
        if (data.itemType == ItemType.Flashlight && data.itemName == "Flashlight")
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
            int baseballCooldown = 5000;
            // Trigger the animation
            if (animator != null)
            {
                if (canUseWeapon)
                    {
                        SpawnHitbox(user);
                        System.Random random = new();
                        int attack = random.Next(0, 3);
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
        if (data.itemType == ItemType.Healing && data.itemName == "Coffee")
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

        //Future Items
    }


    async void UseItem(int cooldownMilliseconds)
    {
            Debug.Log("Skill used!");
            fpc.walkSpeed = 50f;
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

    IEnumerator CooldownRoutine(float duration)
    {
        canUseSkill = false;
        yield return new WaitForSeconds(duration);
        canUseSkill = true;
        Debug.Log("Skill is ready again!");
    }

}