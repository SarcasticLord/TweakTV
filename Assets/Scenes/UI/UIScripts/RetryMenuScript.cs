using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RetryMenuScript : MonoBehaviour
{
    private UIDocument _document;

    [SerializeField] private string TitleScreen = "TitleScreen";

    Button title_button;
    Button retry_button;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        title_button = _document.rootVisualElement.Q("TitleButton") as Button;
        title_button.RegisterCallback<ClickEvent>(ReturnToTitleClicked);
    }

    private void ReturnToTitleClicked(ClickEvent evt)
    {
        Debug.Log("Returning to title");
        SceneManager.LoadScene(TitleScreen);
    }

}
