using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{

    public float spacing = 10f; // Fixed spacing between items
    public bool stackFromBottom = true;
    public int health;

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


