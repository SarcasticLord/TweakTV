using UnityEngine;

public class BeeHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;    
    private ChatDisplay chat;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Call this method to reduce health
    public void TakeDamage(int damageAmount)
    {
        GameObject chatobject = GameObject.Find("Chat");
        chat = chatobject.GetComponent<ChatDisplay>();

        currentHealth -= damageAmount;
        Debug.Log($"Bee took damage: Current Health {currentHealth}");
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
