using UnityEngine;
using UnityEngine.UI;

public class LightSwitch : MonoBehaviour
{
    private bool playerInRange = false;
    public Sprite crosshair;
    public Sprite pickup;
    public Image pickupPrompt;
    private bool isOn = true;
    public Material onMaterial;
    public Material offMaterial;

    private void Update()
    {
        if (playerInRange && pickupPrompt == null)
        {
            pickupPrompt.sprite = pickup;
        }
        else if (!playerInRange && pickupPrompt == null)
        {
            pickupPrompt.sprite = crosshair;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (playerInRange && Input.GetMouseButtonDown(0))
            {
                isOn = !isOn;
                ToggleLights(isOn);
            }
            else if (playerInRange && Input.GetKeyDown(KeyCode.R))
            {
                isOn = !isOn;
                ToggleColorLights(isOn);
            }
        }
    }

    private void ToggleLights(bool state)
    {
        GameObject[] lights = GameObject.FindGameObjectsWithTag("SubwayLight");
        foreach (GameObject lightObj in lights)
        {
            Renderer renderer= lightObj.GetComponent<Renderer>();
            Light lightComponent = lightObj.GetComponentInChildren<Light>();
            if (lightComponent != null)
            {
                lightComponent.enabled = state;
            }
            if (renderer != null)
            {
                Material[] materials = renderer.materials;
                if (materials.Length > 1)
                {
                    materials[1] = state ? onMaterial : offMaterial;
                    renderer.materials = materials;
                }
            }
        }
    }
    private void ToggleColorLights(bool state)
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


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
}
