using UnityEngine;

public class CollectibleScore : MonoBehaviour
{
    public int value;

    private void OnDestroy()
    {
        Singleton.Instance.score += value;
    }
}
