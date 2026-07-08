using UnityEngine;
using UnityEngine.UI;

public class SubwayLightManager : MonoBehaviour
{
    private bool playerInRange = false;
    public GameObject itemPrefab;
    public Sprite crosshair;
    public Sprite pickup;
    public Transform itemSpawn;
    public Image pickupPrompt;
    private bool isOn;
    private Light subwaylight;



    private void Start()
    {
        playerInRange = false;
        subwaylight = GetComponentInChildren<Light>();
    }

    private void Update()
    {

        if (isOn)
        {
            subwaylight.enabled = true;
        }
        else
        {
            subwaylight.enabled = false;
        }
    }
}
