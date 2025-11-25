using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SubwayManager : MonoBehaviour
{
    public bool off = false;
    public LightSwitch lights;
    public Material onMaterial;
    public Material offMaterial;
    public GameObject Exit;
    public string trackedtag;
    public GameObject exitPrefab;
    public Transform spawnPointsContainer;
    public bool exitSpawn = false;
    public Transform streamSniperSpawn;
    public GameObject streamSniper;

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int objectCount = GameObject.FindGameObjectsWithTag(trackedtag).Length;
        Debug.Log("Total GameObjects in scene: " + objectCount);

        // Print the scene name
        Debug.Log("Current Scene: " + currentScene.name);

        if (currentScene.name == "TweakHQ" && !exitSpawn)
        {
            Debug.Log("Keycard Spawned!");
            int childCount = spawnPointsContainer.childCount;
            SpawnExit(childCount);
            return;
        }

        if (objectCount <= 5)
        {
            if (currentScene.name == "AsylumLevel2" && !exitSpawn)
            {
                Debug.Log("You won in the Asylum scene!");
                int childCount = spawnPointsContainer.childCount;

                if (childCount == 0)
                {
                    Debug.LogError("No spawn points found under the parent container!");
                    return;
                }
                SpawnExit(childCount);
            }

            if (objectCount <= 0)
            {
                if (currentScene.name == "subway" && !exitSpawn)
                {
                    Debug.Log("You won in the Subway scene!");
                    ToggleLights();
                    Debug.Log("Lights off");
                    Instantiate(Exit, gameObject.transform.position, gameObject.transform.rotation);
                }
            }
        }
    }
    //Universal
    public void SpawnStreamSniper()
    {
        exitSpawn = true;
        // Pick a random spawn point
        Transform chosenSpawn = streamSniperSpawn;

        // Instantiate the door at the chosen spawn point
        Instantiate(streamSniper, chosenSpawn.position, chosenSpawn.rotation);
    }

    //For the Asylum
    public void SpawnExit(int childCount)
    {
        exitSpawn = true;
        // Pick a random spawn point
        int randomIndex = Random.Range(0, childCount);
        Transform chosenSpawn = spawnPointsContainer.GetChild(randomIndex);
        
        // Instantiate the door at the chosen spawn point
        Instantiate(exitPrefab, chosenSpawn.position, chosenSpawn.rotation);
    }
    //For the Subway
    public void ToggleLights()
    {
        SpawnStreamSniper();
        GameObject[] lights = GameObject.FindGameObjectsWithTag("SubwayLight");
        foreach (GameObject lightObj in lights)
        {
            Renderer renderer = lightObj.GetComponent<Renderer>();
            Light lightComponent = lightObj.GetComponentInChildren<Light>();
            if (lightComponent != null)
            {
                lightComponent.enabled = off;
            }
            if (renderer != null)
            {
                Material[] materials = renderer.materials;
                if (materials.Length > 1)
                {
                    materials[1] = off ? onMaterial : offMaterial;
                    renderer.materials = materials;
                }
            }
        }
    }


}
