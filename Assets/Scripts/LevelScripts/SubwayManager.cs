using UnityEngine;
using UnityEngine.UIElements;

public class SubwayManager : MonoBehaviour
{
    public bool off = false;
    public LightSwitch lights;
    public Material onMaterial;
    public Material offMaterial;
    public GameObject Exit;

    // Update is called once per frame
    void Update()
    {

        int objectCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("Total GameObjects in scene: " + objectCount);

        if (objectCount <= 0)
        {
            ToggleLights();
            Debug.Log("Lights off");
            Instantiate(Exit, gameObject.transform.position, gameObject.transform.rotation);
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
