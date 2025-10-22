using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;    
    //private ChatDisplay chat;

    void Start()
    {
        currentHealth = maxHealth;
        //chat = chatobject.GetComponent<ChatDisplay>();
    }

    // Call this method to reduce health
    public void PlayerTakeDamage(int damageAmount)
    {
        //GameObject chatobject = GameObject.Find("Chat");
        currentHealth -= damageAmount;
        Debug.Log($"Player took damage: Current Health {currentHealth}");
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
