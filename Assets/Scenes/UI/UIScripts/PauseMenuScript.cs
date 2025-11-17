using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuScript : MonoBehaviour
{
    private VisualElement pauseMenu;

    [SerializeField] private string TitleScreen = "TitleScreen";

    bool gamePaused = false;

    private UIDocument _document;

    private Button _StopStreamButton;
    private Button _ResumeStreamButton;
    private Button _ExitPauseButton;

    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        pauseMenu = root.Q<VisualElement>("PauseMenu");
        root.Q<Button>("ResumeStream").clicked += () => ResumeGame();
        root.Q<Button>("ExitButton").clicked += () => ResumeGame();
        root.Q<Button>("EndStream").clicked += () => PauseGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        gamePaused = false;
        pauseMenu.style.display = DisplayStyle.None;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        gamePaused = true;
        pauseMenu.style.display = DisplayStyle.Flex;
    }

    //private void Awake()
    //{
    //    _document = GetComponent<UIDocument>();

    //    _StopStreamButton = _document.rootVisualElement.Q("EndStream") as Button;
    //    _StopStreamButton.RegisterCallback<ClickEvent>(OnStopStreamClick);

    //    _ResumeStreamButton = _document.rootVisualElement.Q("ResumeStream") as Button;
    //    _ResumeStreamButton.RegisterCallback<ClickEvent>(OnResumeClick);

    //    _ExitPauseButton = _document.rootVisualElement.Q("QuitGameButton") as Button;
    //    _ExitPauseButton.RegisterCallback<ClickEvent>(OnResumeClick);


    //}

    //private void OnDisable()
    //{
    //    _StopStreamButton.UnregisterCallback<ClickEvent>(OnStopStreamClick);
    //    _ResumeStreamButton.UnregisterCallback<ClickEvent>(OnResumeClick);
    //    _ExitPauseButton.UnregisterCallback<ClickEvent>(OnResumeClick);
    //}

    //private void OnStopStreamClick(ClickEvent evt)
    //{
    //    Debug.Log("Returning to title");
    //    SceneManager.LoadScene(TitleScreen);
    //}

    //private void OnResumeClick(ClickEvent evt)
    //{
    //    Debug.Log("Continuing Game");
    //    Destroy(PauseContainer);
    //}
}
