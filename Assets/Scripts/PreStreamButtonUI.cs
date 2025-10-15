using UnityEngine;
using UnityEngine.SceneManagement;

public class PreStreamButtonUI : MonoBehaviour
{
    [SerializeField] private string selectScene = "SelectScene";

    public void BackButton()
    {
        SceneManager.LoadScene(selectScene);
    }
}
