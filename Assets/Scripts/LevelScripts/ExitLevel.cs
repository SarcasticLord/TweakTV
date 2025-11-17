using EasyPeasyFirstPersonController;
using UnityEngine;

public class ExitGame : MonoBehaviour
{ 
    private GameObject weapons;
    private GameObject hud;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weapons = GameObject.FindGameObjectWithTag("Hotbar");
        hud = GameObject.FindGameObjectWithTag("HUD");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WholePlayer"))
        {
            Debug.Log("You beat the level!");
            other.GetComponent<FirstPersonController>().enabled = false;
            weapons.SetActive(false);
            hud.SetActive(false);

        }
    }


    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("WholePlayer"))
        {
            other.transform.position = transform.position;
            other.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
