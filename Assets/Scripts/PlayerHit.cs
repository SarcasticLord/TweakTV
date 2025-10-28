using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public int damageAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") == true)
        {
            Destroy(other);
            PlayerHealth playerHealth = FindAnyObjectByType<PlayerHealth>();
            Debug.Log("Player Hit!");
            damageAmount = other.GetComponent<BeeHitbox>().damage;
            Debug.Log($"Damage Taken: {damageAmount}");
            playerHealth.PlayerTakeDamage(damageAmount);
        }
    }

    // Update is called once per frame

}
