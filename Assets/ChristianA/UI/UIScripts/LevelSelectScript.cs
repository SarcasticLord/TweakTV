using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelSelectScript : MonoBehaviour
{
    [SerializeField] private string TitleScreen = "TitleScreen";
    [SerializeField] private string PreStreamScene = "PreStreamScene";

    private UIDocument _document;

    private Button _back;
    private Button _start;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _back = _document.rootVisualElement.Q("BackButton") as Button;
        _back.RegisterCallback<ClickEvent>(OnBackClick);

        _start = _document.rootVisualElement.Q("PrepareStreamButton") as Button;
        _start.RegisterCallback<ClickEvent>(OnPrepStreamClick);
    }

    private void OnDisable()
    {
        _back.UnregisterCallback<ClickEvent>(OnBackClick);
    }

    private void OnBackClick(ClickEvent evt)
    {
        Debug.Log("Returning to title");
        SceneManager.LoadScene(TitleScreen);
    }

    private void OnPrepStreamClick(ClickEvent evt)
    {
        Debug.Log("Preparing Stream!");
        SceneManager.LoadScene(PreStreamScene);
    }
}
