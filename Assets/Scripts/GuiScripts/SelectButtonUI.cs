using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectButtonUI : MonoBehaviour
{

    [SerializeField] private string TitleScreen = "TitleScreen";
    [SerializeField] private string StartStream = "PreStreamScene";
    public void BackButton()
    {
        SceneManager.LoadScene(TitleScreen);
    }

    public void StartStreamButton()
    {
        SceneManager.LoadScene(StartStream);
    }

}
