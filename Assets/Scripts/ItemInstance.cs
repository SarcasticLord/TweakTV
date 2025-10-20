
// ItemInstance.cs
using NUnit.Framework.Interfaces;
using System;
using Unity;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class ItemInstance
{
    public ItemData data;
    public int currentDurability;
    public GameObject player;
    public GameObject worldObject; // The actual flashlight GameObject in the scene
    public GameObject instance;
    private ChatDisplay chat;


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

            if (data.itemType == ItemType.Weapon)
            {

            Combat(); //Changes chat to combat
            Animator animator = worldObject.GetComponent<Animator>();
            // Trigger the animation
            if (animator != null)
                {
                SpawnHitbox(user);
                    System.Random random = new();
                    int attack = random.Next(0, 3);
                    if (attack == 0)
                        {
                            animator.SetTrigger("Attack1");
                        }
                    else if(attack == 1)
                        {
                            animator.SetTrigger("Attack2");
                        }
                    else if(attack == 2)
                        {
                            animator.SetTrigger("Attack3");
                        }
                    Debug.Log($"Played weapon animation for {data.itemName}");
                    currentDurability--;
                }
            }

            //Add further item types here

        else
        {
            currentDurability--;
        }
        
    }

    public void Combat()
    {
        chat.ChangeChatSource("Combat");
    }

    public void Idle()
    {
        chat.ChangeChatSource("Idle");
    }

}