using UnityEngine;

public class Beartrap : MonoBehaviour
{

    public GameObject closedTrap;
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(closedTrap, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}
