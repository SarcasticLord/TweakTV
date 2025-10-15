using UnityEngine;
using UnityEngine.SceneManagement;

public class StatButtonUI : MonoBehaviour
{
    [SerializeField] private string TitleScreen = "TitleScreen";

    public void BackButton()
    {
        SceneManager.LoadScene(TitleScreen);
    }
}