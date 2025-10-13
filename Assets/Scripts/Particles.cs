using UnityEngine;
using System.Collections;

public class BloodDecalSpawner : MonoBehaviour
{
    public GameObject bloodDecalPrefab; // Assign in Inspector
    public float decalLifetime = 5f;

    void OnParticleCollision(GameObject other)
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ParticleCollisionEvent[] collisionEvents = new ParticleCollisionEvent[ps.GetSafeCollisionEventSize()];
        int numEvents = ps.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numEvents; i++)
        {
            Vector3 pos = collisionEvents[i].intersection;
            Quaternion rot = Quaternion.LookRotation(collisionEvents[i].normal);

            GameObject decal = Instantiate(bloodDecalPrefab, pos, rot);
            StartCoroutine(FadeAndDestroy(decal, decalLifetime));
        }
    }

    IEnumerator FadeAndDestroy(GameObject obj, float duration)
    {
        float elapsed = 0f;
        Material mat = obj.GetComponent<Renderer>().material;
        Color startColor = mat.color;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            mat.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(obj);
    }
}