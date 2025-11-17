using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PauseContainer;

    [SerializeField] private string TitleScreen = "TitleScreen";

    private UIDocument _document;

    private Button _StopStreamButton;
    private Button _ResumeStreamButton;
    private Button _ExitPauseButton;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _StopStreamButton = _document.rootVisualElement.Q("EndStream") as Button;
        _StopStreamButton.RegisterCallback<ClickEvent>(OnStopStreamClick);

        _ResumeStreamButton = _document.rootVisualElement.Q("ResumeStream") as Button;
        _ResumeStreamButton.RegisterCallback<ClickEvent>(OnResumeClick);

        _ExitPauseButton = _document.rootVisualElement.Q("QuitGameButton") as Button;
        _ExitPauseButton.RegisterCallback<ClickEvent>(OnResumeClick);

       
    }

    private void OnDisable()
    {
        _StopStreamButton.UnregisterCallback<ClickEvent>(OnStopStreamClick);
        _ResumeStreamButton.UnregisterCallback<ClickEvent>(OnResumeClick);
        _ExitPauseButton.UnregisterCallback<ClickEvent>(OnResumeClick);
    }

    private void OnStopStreamClick(ClickEvent evt)
    {
        Debug.Log("Returning to title");
        SceneManager.LoadScene(TitleScreen);
    }

    private void OnResumeClick(ClickEvent evt)
    {
        Debug.Log("Continuing Game");
        Destroy(PauseContainer);
    }
}
