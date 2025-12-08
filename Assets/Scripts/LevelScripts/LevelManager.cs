using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static EnemyStates;

public class LevelManager : MonoBehaviour
{
    public bool off = false;
    public LightSwitch lights;
    public Material onMaterial;
    public Material offMaterial;
    public GameObject Exit;
    public string trackedtag;
    public GameObject Keycard;
    public Transform spawnPointsContainer;
    public bool exitSpawn = false;
    public Transform streamSniperSpawn;
    public GameObject streamSniper;
    public Transform doorPivot;
    public float openAngle = 90f;
    public float speed = 2f;
    public bool isOpen = false;
    private bool keycardSpawned = false;
    private bool doorOpen = false;
    public float timeToSpawn;
    private float gameTime;
    private bool sniperSpawned = false;
    public int minObjects = 1;
    public float floatScore;



    // Update is called once per frame
    public void Update()
    {
        gameTime = GameObject.FindGameObjectWithTag("WholePlayer").GetComponent<GameStats>().elapsedTime;
        Scene currentScene = SceneManager.GetActiveScene();
        int objectCount = GameObject.FindGameObjectsWithTag(trackedtag).Length;
        Debug.Log("Total GameObjects in scene: " + objectCount);

        // Print the scene name
        Debug.Log("Current Scene: " + currentScene.name);

        if (gameTime >= timeToSpawn && !sniperSpawned)
        {
            SpawnStreamSniper();
            sniperSpawned=true;
        }

        //TweakHQ-Spawns Keycard
        if (currentScene.name == "TweakHQ")
        {
            floatScore -= 1f *Time.deltaTime;
            if(!exitSpawn && keycardSpawned == false){
                floatScore = 300f;
                Debug.Log("Keycard Spawned!");
                int childCount = spawnPointsContainer.childCount;
                SpawnKeycard(childCount);
                keycardSpawned = true;
            }
        }

        if (objectCount <= minObjects)
        {
            Debug.Log("Objective Completed!");
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
            if (currentScene.name == "subway")
            {
                Debug.Log("You won in the Subway scene!");

                if (!exitSpawn)
                {
                    ToggleLights();
                    SpawnStreamSniper();
                    Debug.Log("Lights off");
                    Instantiate(Exit, gameObject.transform.position, gameObject.transform.rotation);
                    exitSpawn = true;
                }
            }

            if (objectCount <= 0)
            {
               
                //TweakHQ
                if(currentScene.name == "TweakHQ")
                {
                    isOpen = true;
                    Singleton.Instance.score = Mathf.FloorToInt(floatScore);
                    float targetAngle = openAngle;
                    doorPivot.localRotation = Quaternion.Lerp(
                        doorPivot.localRotation,
                        Quaternion.Euler(0, targetAngle, 0),
                        Time.deltaTime * speed
                    );
                }
            }
            if (isOpen && doorPivot != null && !exitSpawn)
            {
                Debug.Log("You won in the TweakHQ scene!");
                GameObject spawnedExit = GameObject.Instantiate(Exit, gameObject.transform.position, gameObject.transform.rotation);
                spawnedExit.transform.SetParent(gameObject.transform, true);
                exitSpawn = true;
            }
        }
    }

    //Universal
    public void SpawnStreamSniper()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        exitSpawn = true;
        // Pick a random spawn point
        Transform chosenSpawn = streamSniperSpawn;
        if(currentScene.name == "subway")
        {
            ToggleColorLights(true);
        }
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
        Instantiate(Exit, chosenSpawn.position, chosenSpawn.rotation);
    }
    public void SpawnKeycard(int childCount)
    {
        // Pick a random spawn point
        int randomIndex = Random.Range(0, childCount);
        Transform chosenSpawn = spawnPointsContainer.GetChild(randomIndex);

        // Instantiate the door at the chosen spawn point
        Instantiate(Keycard, chosenSpawn.position, chosenSpawn.rotation);
    }

    //For the Subway
    public void ToggleLights()
    {
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

    public void ToggleColorLights(bool state)
    {
        GameObject[] lights = GameObject.FindGameObjectsWithTag("SubwayLight");
        foreach (GameObject lightObj in lights)
        {
            Light lightComponent = lightObj.GetComponentInChildren<Light>();
            if (lightComponent != null)
            {
                lightComponent.color = state ? new Color(1f, 0f, 0f) : new Color(250f / 255f, 1f, 215f / 255f);
                lightComponent.intensity = state ? 500 : 50;
            }
        }
    }
}
