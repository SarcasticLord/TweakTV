using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RetryMenuScript : MonoBehaviour
{
    private UIDocument _document;

    [SerializeField] private string TitleScreen = "TitleScreen";

    Button title_button;
    Button retry_button;


    void OnEnable()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        Time.timeScale = 1f; // just in case you        Time.timeScale = 1f; // just in case you paused elsewhere
    }

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        title_button = _document.rootVisualElement.Q("TitleButton") as Button;
        title_button.RegisterCallback<ClickEvent>(ReturnToTitleClicked);
        retry_button = _document.rootVisualElement.Q("RetryButton") as Button;
        retry_button.RegisterCallback<ClickEvent>(RestartClicked);
    }

    private void ReturnToTitleClicked(ClickEvent evt)
    {
        Debug.Log("Returning to title");
        SceneManager.LoadScene(TitleScreen);
    }

    private void RestartClicked(ClickEvent evt) 
    {
        Debug.Log("Restarting Level");
        Singleton.Instance.score = 0;
        Singleton.Instance.time = 0;
        SceneManager.LoadScene(Singleton.Instance.currentLevel);
    }

}
