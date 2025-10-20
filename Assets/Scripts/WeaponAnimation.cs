
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    public ItemInstance itemInstance;
    public GameObject user;

    // This method will be called by the animation event
    public void TriggerHitbox()
    {
        if (itemInstance != null && user != null)
        {
            itemInstance.SpawnHitbox(user);
        }
        else
        {
            Debug.LogWarning("ItemInstance or user not assigned in WeaponAnimationEvents.");
        }
    }
}
