using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneLoader : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // Replace with your target scene name

    void Start()
    {
        nextSceneName = Singleton.Instance.targetLevel;
        // Immediately load the specified scene
        SceneManager.LoadScene(nextSceneName);
    }
}
