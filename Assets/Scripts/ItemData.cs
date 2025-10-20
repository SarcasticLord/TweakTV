using System.ComponentModel;
using Unity;
using UnityEngine;


public enum ItemType
{
    Healing,
    Weapon,
    SpeedUp,
    Flashlight,
    Throwable,
}


[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]

public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int maxDurability;
    public ItemType itemType;
    public Light flashlightLight;
    public Animator weaponAnimator;
    public GameObject hitbox;
    public float offset;

}
