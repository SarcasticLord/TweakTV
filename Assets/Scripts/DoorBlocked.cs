using System.Collections.Generic;
using UnityEngine;

public class DoorBlocked : MonoBehaviour
{
    private HashSet<GameObject> boards = new HashSet<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Crowable"))
        {

            boards.Add(other.gameObject);
            Debug.Log($"Door is blocked by {other.name}");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Crowable"))
        {
            boards.Remove(other.gameObject);
            Debug.Log("Board removed: " + other.name);
        }

    }

        // Update is called once per frame
    void Update()
    {
        if (boards.Count > 0) //Door is blocked
        {
            Debug.Log("Door is currently BLOCKED");
        }
        else //Door is clear
        {
            Debug.Log("Door is CLEAR");
            //Change later
            Destroy(gameObject);
        }
     }
}
