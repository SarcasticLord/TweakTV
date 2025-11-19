using EasyPeasyFirstPersonController;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{ 
    private GameObject weapons;
    private GameObject hud;
    private Scene currentScene;
    public GameObject exitPrefab;
    private Transform spawnPointsContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        weapons = GameObject.FindGameObjectWithTag("Hotbar");
        hud = GameObject.FindGameObjectWithTag("HUD");

        if (spawnPointsContainer == null)
        {
            Debug.LogError("No spawn points container assigned.");
            return;
        }
        int childCount = spawnPointsContainer.childCount;

        if (childCount == 0)
        {
            Debug.LogError("No spawn points found under the parent container!");
            return;
        }

        // Pick a random spawn point
        int randomIndex = Random.Range(0, childCount);
        Transform chosenSpawn = spawnPointsContainer.GetChild(randomIndex);

        // Instantiate the door at the chosen spawn point
        Instantiate(exitPrefab, chosenSpawn.position, chosenSpawn.rotation);
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
            if (currentScene.name == "subway")
            {
                other.transform.position = transform.position;
                other.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            }
            if (currentScene.name == "subway")
            {
                other.transform.position = transform.position;
                other.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
