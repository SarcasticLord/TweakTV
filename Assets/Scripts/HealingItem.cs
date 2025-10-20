using UnityEngine;


[CreateAssetMenu(fileName = "NewHealItem", menuName = "Inventory/Heal Item")]
public class HealItem : ItemData, IUsableItem
{
    public int healAmount;

    public void Use(GameObject user)
    {
        Debug.Log($"Healing {user.name} for {healAmount} HP.");
        // Example: user.GetComponent<PlayerHealth>().Heal(healAmount);
    }
}
