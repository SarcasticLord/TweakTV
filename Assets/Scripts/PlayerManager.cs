using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;
    private PlayerHealthDisplay healthDisplay;
    //private ChatDisplay chat;

    void Start()
    {
        healthDisplay = GetComponent<PlayerHealthDisplay>();
        currentHealth = maxHealth;
        healthDisplay.health = currentHealth;
    }

    // Call this method to reduce health
    public void PlayerTakeDamage(int damageAmount)
    {
        //GameObject chatobject = GameObject.Find("Chat");
        currentHealth -= damageAmount;
        healthDisplay.health = currentHealth;
        Debug.Log($"Player took damage: Current Health {currentHealth}");
        healthDisplay.UpdateLayout();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // You can add effects or animations here before destroying
        Destroy(gameObject);
    }
}
