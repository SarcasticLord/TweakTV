using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public int value;


    public GameObject confetti;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectibles"))
        {
            Singleton.Instance.score += value;
            Destroy(other.gameObject);
            Instantiate(confetti, gameObject.transform.position, Quaternion.Euler(-90,0,0));
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
