using EasyPeasyFirstPersonController;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    private BoxCollider collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WholePlayer"))
        {
            Debug.Log("You beat the level!");
            other.GetComponent<FirstPersonController>().enabled = false;
            other.transform.rotation = Quaternion.Euler(0f,270f,0f);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("WholePlayer"))
        {
            other.transform.position = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
