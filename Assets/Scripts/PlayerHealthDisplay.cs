using System.Security.Cryptography;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{

    public float spacing = 10f; // Fixed spacing between items
    public bool stackFromBottom = true;
    public GameObject healthPoint;
    public int health;

    private void Start()
    {
        ;
    }

    public void CreateHealth(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject bar = Instantiate(healthPoint, transform);
            RectTransform rt = bar.GetComponent<RectTransform>();

            // Ensure correct anchoring and pivot
            rt.pivot = new Vector2(0.5f, 0f);
            rt.anchorMin = new Vector2(0.5f, 0f);
            rt.anchorMax = new Vector2(0.5f, 0f);
        }
    }

    public void UpdateLayout()
    {
        int count = health;
        for (int i = 0; i < count; i++)
        {
            RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
            if (child == null) continue;

            float yOffset = (child.sizeDelta.y + spacing) * i;
            if (stackFromBottom)
            {
                child.anchoredPosition = new Vector2(0, yOffset);
            }
            else
            {
                child.anchoredPosition = new Vector2(0, -yOffset);
            }
        }
    }

    void Update()
    {
        UpdateLayout();
    }

    // Optional: Call UpdateLayout() whenever items are added/removed
}


