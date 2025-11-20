using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public GameObject confetti;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectibles"))
        {
            Destroy(other.gameObject);
            Instantiate(confetti, gameObject.transform.position, Quaternion.Euler(-90,0,0));

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
