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
    

    // Update is called once per frame
    void Update()
    {

        Scene currentScene = SceneManager.GetActiveScene();
        int objectCount = GameObject.FindGameObjectsWithTag(trackedtag).Length;
        Debug.Log("Total GameObjects in scene: " + objectCount);

        // Print the scene name
        Debug.Log("Current Scene: " + currentScene.name);

        // Or check by name
       

        if (objectCount <= 0)
        {
            if (currentScene.name == "AsylumLevel2")
            {
                Debug.Log("You won in the Asylum scene!");
                Exit.SetActive(true);
            }

            if (currentScene.name == "subway")
            {
                Debug.Log("You won in the Subway scene!");
                ToggleLights();
                Debug.Log("Lights off");
                Instantiate(Exit, gameObject.transform.position, gameObject.transform.rotation);
            }
        }
    }

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


}
