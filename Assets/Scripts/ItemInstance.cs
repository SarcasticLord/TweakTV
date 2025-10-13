
// ItemInstance.cs
using NUnit.Framework.Interfaces;
using Unity;
using UnityEngine;

public class ItemInstance
{
    public ItemData data;
    public int currentDurability;
    public GameObject player;
    public GameObject worldObject; // The actual flashlight GameObject in the scene

    public bool IsBroken => currentDurability <= 0;


    public ItemInstance(ItemData itemData, GameObject instance = null)
    {
        data = itemData;
        currentDurability = itemData.maxDurability;
        worldObject = instance;
    }

    public void Use(GameObject user)
    {
        if (currentDurability <= 0)
        {
            Debug.Log($"{data.itemName} is broken or out of uses.");
            return;
        }

        currentDurability--;
        {

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
        }
    }
}