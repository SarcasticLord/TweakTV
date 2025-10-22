using UnityEngine;

public class BeeHitbox : MonoBehaviour
{
    //public float forceAmount = 10f;
    //public Vector3 forceDirection;
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") == true)
        {
            Debug.Log("They hit the Player!");
            Destroy(gameObject, .01f);
        }
    }
    private void Start()
    {
        Destroy(gameObject, .5f);
    }
}
