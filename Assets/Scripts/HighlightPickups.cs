using UnityEngine;

public class ObjectHighlighter : MonoBehaviour
{
    public float highlightDistance = 3f;
    public Color highlightColor = Color.yellow;
    public string highlightTag = "Highlightable";

    private GameObject currentHighlighted;
    private Material originalMaterial;
    private Color originalColor;

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, highlightDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag(highlightTag))
            {
                if (currentHighlighted != hitObject)
                {
                    ClearHighlight();

                    Renderer renderer = hitObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        originalMaterial = renderer.material;
                        originalColor = originalMaterial.color;

                        Material highlightMat = new Material(originalMaterial);
                        highlightMat.color = highlightColor;
                        renderer.material = highlightMat;

                        currentHighlighted = hitObject;
                    }
                }
            }
            else
            {
                ClearHighlight();
            }
        }
        else
        {
            ClearHighlight();
        }
    }

    void ClearHighlight()
    {
        if (currentHighlighted != null)
        {
            Renderer renderer = currentHighlighted.GetComponent<Renderer>();
            if (renderer != null && originalMaterial != null)
            {
                Material resetMat = new Material(originalMaterial);
                resetMat.color = originalColor;
                renderer.material = resetMat;
            }

            currentHighlighted = null;
            originalMaterial = null;
        }
    }
}
