using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PreStreamScript : MonoBehaviour
{
    public GameController controller;

    [SerializeField] private string selectScene = "SelectScene";

    private UIDocument _document;

    private Button _back;
    private Button _start;

    [SerializeField] private string SelectedLevel = null;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _back = _document.rootVisualElement.Q("BackButton") as Button;
        _back.RegisterCallback<ClickEvent>(OnBackClick);

        _start = _document.rootVisualElement.Q("PrepareStreamButton") as Button;
        _start.RegisterCallback<ClickEvent>(OnGameStartClick);
    }

    private void OnDisable()
    {
        _back.UnregisterCallback<ClickEvent>(OnBackClick);
    }

    private void OnBackClick(ClickEvent evt)
    {
        Debug.Log("Returning to title");
        SceneManager.LoadScene(selectScene);
    }

    private void OnGameStartClick(ClickEvent evt)
    {
        SelectedLevel = controller.GetSelectedLevel();
        Debug.Log(SelectedLevel);
        Singleton.Instance.currentLevel = SelectedLevel;
        SceneManager.LoadScene(SelectedLevel);
    }
}
