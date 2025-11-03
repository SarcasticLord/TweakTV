using EasyPeasyFirstPersonController;
using System.Collections;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using Image = UnityEngine.UI.Image;

public class PlayerHealth : MonoBehaviour
{

    public float spacing = 10f; // Fixed spacing between items
    public bool stackFromBottom = true;
    public GameObject healthPoint;
    public GameObject player;
    public Camera camera;
    private int health;
    public int maxHealth = 4;
    private Image imageComponent;
    private bool lowHealth;
    public float fallDuration = 1f; // Duration of the fall animation
    private Vector3 targetPosition;
    private Quaternion targetRotation;



    private void Start()
    {

        targetPosition = transform.position + new Vector3(0, -2f, 0);
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, 90f));

        if (imageComponent == null)
        {
            imageComponent = GetComponent<Image>();
        }
        health = maxHealth;
        CreateHealth(health-1);
        UpdateLayout();

    }
    void CreateHealth(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject bar = Instantiate(healthPoint, transform);
            RectTransform rt = bar.GetComponent<RectTransform>();

            // Ensure correct anchoring and pivot
            rt.pivot = new Vector2(0.35f, 0f);
            rt.anchorMin = new Vector2(0.35f, 0.25f);
            rt.anchorMax = new Vector2(0.35f, 0.25f);
        }
        Debug.Log("Health Created.");
    }
    public void PlayerTakeDamage(int damageAmount)
    {
        //GameObject chatobject = GameObject.Find("Chat");
        health -= damageAmount;
        RemoveTopBar();
        if (health <= 1)
        {
            StartCoroutine(LowHealth(.2f));
            if (health <= 0)
            {
                Death();
            }
        }
        Debug.Log($"Player took damage: Current Health {health}");
    }

    public void RemoveTopBar()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        Transform topBar = transform.GetChild(childCount - 1);
        Destroy(topBar.gameObject);

        UpdateLayout(); // Reposition remaining bars
    }
    public void Death()
    {
        StopAllCoroutines();
        FirstPersonController fps = player.GetComponent<FirstPersonController>();
        fps.enabled = false;
        StartCoroutine(FallToSide());
    }

    IEnumerator FallToSide()
    {
        Vector3 startPos = camera.transform.position;
        Quaternion startRot = camera.transform.rotation;
        float elapsed = 0f;

        while (elapsed < fallDuration)
        {
            camera.transform.position = Vector3.Lerp(startPos, targetPosition, elapsed / fallDuration);
            camera.transform.rotation = Quaternion.Lerp(startRot, targetRotation, elapsed / fallDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final position and rotation are set
        camera.transform.position = targetPosition;
        camera.transform.rotation = targetRotation;
        StopAllCoroutines();
    }

    public void UpdateLayout()
    {
        float currentX = 0f;

        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
            if (child == null) continue;

            child.anchoredPosition = new Vector2(currentX, 0);
            currentX += child.sizeDelta.x + spacing;
        }

    }
    private IEnumerator LowHealth(float duration)
    {
        //int time = 10;
        while (!lowHealth)
        {

            imageComponent.color = Color.red;
            yield return new WaitForSeconds(duration);

            imageComponent.color = Color.white;
            yield return new WaitForSeconds(duration);
        }

    }

    void Update()
    {
        UpdateLayout();
    }

    // Optional: Call UpdateLayout() whenever items are added/removed
}


