using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    public PlayerHealthDisplay healthDisplay;
    //private ChatDisplay chat;

    void Start()
    {
        healthDisplay = GetComponent<PlayerHealthDisplay>();
        Debug.Log($"Got Health Display {healthDisplay}");
        currentHealth = maxHealth;
        healthDisplay.CreateHealth(currentHealth);
        AdjustHealth();
    }

    // Call this method to reduce health
    public void PlayerTakeDamage(int damageAmount)
    {
        //GameObject chatobject = GameObject.Find("Chat");
        currentHealth -= damageAmount;
        AdjustHealth();
        Debug.Log($"Player took damage: Current Health {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void AdjustHealth()
    {
        healthDisplay.health = currentHealth;
        healthDisplay.UpdateLayout();
        Debug.Log("Updated Health.");
    }
    private void Die()
    {
        // You can add effects or animations here before destroying
        Destroy(gameObject);
    }
}
