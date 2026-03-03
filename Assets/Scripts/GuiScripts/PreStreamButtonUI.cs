using UnityEngine;
using UnityEngine.SceneManagement;

public class PreStreamButtonUI : MonoBehaviour
{
    [SerializeField] private string selectScene = "SelectScene";
    [SerializeField] private string asylumScene = "AsylumLevel2";

    public void BackButton()
    {
        SceneManager.LoadScene(selectScene);
    }

    public void StartStreamButton()
    {
        SceneManager.LoadScene(asylumScene);   
    }

}
