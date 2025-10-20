using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public float forceAmount = 10f;
    public Vector3 forceDirection;

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log($"Hitbox triggered with {other.name}");
        Rigidbody rb = other.attachedRigidbody;

        if (rb != null)
        {
            rb.AddForce(forceDirection * forceAmount, ForceMode.Impulse);
            Debug.Log($"Applied force to {other.name}");
        }
        // Check if the collided object has the EnemyHealth component


    }
}
