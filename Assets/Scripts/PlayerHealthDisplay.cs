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
    public AudioSource deathSound;
    public float spacing = 10f; // Fixed spacing between items
    public bool stackFromBottom = true;
    public GameObject healthPoint;
    public GameObject player;
    public bool playerIsDead = false;
    public int health;
    public int maxHealth = 4;
    private Image imageComponent;
    public Image deathScreen;
    private bool lowHealth;
    public float fallDuration = 1f; // Duration of the fall animation
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private GameObject weapons;
    private GameObject hud;


    private void Start()
    {
        weapons = GameObject.FindGameObjectWithTag("Hotbar");
        hud = GameObject.FindGameObjectWithTag("HUD");
        if (deathScreen != null)
        {
            deathScreen.enabled = false;
        }
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
        for (int i = 0; i < damageAmount; i++)
        {
            RemoveTopBar();
        }
        if (health <= 1)
        {
            StartCoroutine(LowHealth(.2f));
            if (health <= 0)
            {
                playerIsDead = true;
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
        StartCooldown();
        weapons.SetActive(false);
        hud.SetActive(false);
        if (deathScreen != null)
        {
            deathScreen.enabled = true;
        }
        deathSound.Play();
        FirstPersonController fps = player.GetComponent<FirstPersonController>();
        fps.enabled = false;
        targetPosition = player.transform.position + new Vector3(0, -1.3f, 0);
        targetRotation = Quaternion.Euler(player.transform.eulerAngles + new Vector3(-25f, 0, 40f));
        StartCoroutine(FallToSide());
    }

    IEnumerator FallToSide()
    {
        Vector3 startPos = player.transform.position;
        Quaternion startRot = player.transform.rotation;
        float elapsed = 0f;

        while (elapsed < fallDuration)
        {

            elapsed += Time.deltaTime;
            float t = elapsed / fallDuration;
            player.transform.position = Vector3.Lerp(startPos, targetPosition, t);
            player.transform.rotation = Quaternion.Lerp(startRot, targetRotation, t);
            yield return null;
        }

        // Ensure final position and rotation are set
        player.transform.position = targetPosition;
        player.transform.rotation = targetRotation;
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


    public void StartCooldown()
    {
        StartCoroutine(CooldownAndChangeScene());

    }

    private IEnumerator CooldownAndChangeScene()
    {

        yield return new WaitForSeconds(2);
        StartCoroutine(PlayerCooldown()); // Replace with your scene name
    }
    private IEnumerator PlayerCooldown()
    {
        GameStats playerTransition = player.GetComponent<GameStats>();
        yield return new WaitForSeconds(2f);
        playerTransition.EndLevel();
    }
}


